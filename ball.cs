using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class StoneController : MonoBehaviour
{
    public float powerMultiplier = 10f;
    public float maxDragDistance = 5f;
    public float minDragDistance = 0.3f;

    private Rigidbody rb;
    private LineRenderer line;

    private bool dragging;

    private Vector3 dragStartMouse;
    private Vector3 dragCurrentMouse;

    private Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();

        line.enabled = false;

        cam = Camera.main;
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
    }

    void StartDrag()
    {
        // 動いている間はショット禁止
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                dragging = true;

                dragStartMouse = Input.mousePosition;

                line.enabled = true;
            }
        }
    }

    void UpdateDrag()
    {
        dragCurrentMouse = Input.mousePosition;

        Vector3 mouseDelta =
            dragCurrentMouse - dragStartMouse;

        Vector3 cameraForward = cam.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = cam.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 shotDirection =
            (-mouseDelta.y * cameraForward) +
            (-mouseDelta.x * cameraRight);

        float power =
            Mathf.Clamp01(
                mouseDelta.magnitude /
                (maxDragDistance * 100f)
            );

        DrawArrow(shotDirection, power);
    }

    void Release()
    {
        if (!dragging)
            return;

        dragging = false;

        line.enabled = false;

        Vector3 mouseDelta =
            dragCurrentMouse - dragStartMouse;

        float dragDistance =
            mouseDelta.magnitude / 100f;

        if (dragDistance < minDragDistance)
        {
            return;
        }

        Vector3 cameraForward = cam.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = cam.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 direction =
            (-mouseDelta.y * cameraForward) +
            (-mouseDelta.x * cameraRight);

        direction.Normalize();

        float power =
            Mathf.Clamp(
                dragDistance,
                0,
                maxDragDistance
            );

        // 前方向へ力
        rb.AddForce(
            direction * power * powerMultiplier,
            ForceMode.Impulse
        );

        // 転がるように回転
        Vector3 rotationAxis =
            Vector3.Cross(Vector3.up, direction);

        rb.AddTorque(
            rotationAxis * power * 5f,
            ForceMode.Impulse
        );
    }

    void DrawArrow(Vector3 direction, float power)
    {
        if (direction.magnitude < 0.01f)
            return;

        Vector3 start =
            transform.position + Vector3.up * 0.3f;

        float arrowLength =
            Mathf.Lerp(0.5f, 4f, power);

        Vector3 end =
            start +
            direction.normalized * arrowLength;

        line.positionCount = 2;

        line.SetPosition(0, start);
        line.SetPosition(1, end);

        Color c;

        if (power < 0.33f)
        {
            c = Color.blue;
        }
        else if (power < 0.66f)
        {
            c = Color.yellow;
        }
        else
        {
            c = Color.red;
        }

        line.startColor = c;
        line.endColor = c;
    }
}