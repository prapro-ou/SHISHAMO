using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StoneController : MonoBehaviour
{
    [Header("ショット設定")]
    public float powerMultiplier = 10f;
    public float maxDragDistance = 5f;
    public float minDragDistance = 0.3f;

    [Header("回転設定")]
    public float rotationPower = 5f;

    [Header("落下判定")]
    public float fallLimitY = -10f;

    private Rigidbody rb;
    private Camera cam;

    private bool dragging;

    private Vector3 dragStartMouse;
    private Vector3 dragCurrentMouse;

    private Vector3 lastShotPosition;
    private Quaternion lastShotRotation;

    public bool IsDragging => dragging;
    public float CurrentPower { get; private set; }
    public Vector3 CurrentDirection { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        lastShotPosition = transform.position;
        lastShotRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag();
        }

        if (dragging)
        {
            UpdateDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Release();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReturnToPreviousShot();
        }

        if (transform.position.y < fallLimitY)
        {
            ReturnToPreviousShot();
        }
    }

    void StartDrag()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            return;
        }

        Ray ray =
            cam.ScreenPointToRay(
                Input.mousePosition
            );

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                dragging = true;
                dragStartMouse = Input.mousePosition;
            }
        }
    }

    void UpdateDrag()
    {
        dragCurrentMouse = Input.mousePosition;

        Vector3 mouseDelta =
            dragCurrentMouse - dragStartMouse;

        Vector3 cameraForward =
            cam.transform.forward;

        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight =
            cam.transform.right;

        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 direction =
            (-mouseDelta.y * cameraForward) +
            (-mouseDelta.x * cameraRight);

        direction.Normalize();

        // 坂道の法線を取得
        if (Physics.Raycast(
            transform.position + Vector3.up * 0.5f,
            Vector3.down,
            out RaycastHit hit,
            3f))
        {
            direction =
                Vector3.ProjectOnPlane(
                    direction,
                    hit.normal
                ).normalized;
        }

        CurrentDirection = direction;

        CurrentPower =
            Mathf.Clamp01(
                mouseDelta.magnitude /
                (maxDragDistance * 100f)
            );
    }

    void Release()
    {
        if (!dragging)
            return;

        dragging = false;

        Vector3 mouseDelta =
            dragCurrentMouse - dragStartMouse;

        float dragDistance =
            mouseDelta.magnitude / 100f;

        if (dragDistance < minDragDistance)
            return;

        lastShotPosition = transform.position;
        lastShotRotation = transform.rotation;

        Vector3 direction =
            CurrentDirection.normalized;

        float power =
            Mathf.Clamp(
                dragDistance,
                0f,
                maxDragDistance
            );

        rb.AddForce(
            direction *
            power *
            powerMultiplier,
            ForceMode.Impulse
        );

        Vector3 rotationAxis =
            Vector3.Cross(
                Vector3.up,
                direction
            );

        rb.AddTorque(
            rotationAxis *
            power *
            rotationPower,
            ForceMode.Impulse
        );

        CurrentPower = 0f;
    }

    void ReturnToPreviousShot()
    {
        dragging = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = lastShotPosition;
        transform.rotation = lastShotRotation;

        CurrentPower = 0f;
        CurrentDirection = Vector3.zero;
    }

    public bool CanShoot()
    {
        return rb.linearVelocity.magnitude < 0.1f;
    }
}