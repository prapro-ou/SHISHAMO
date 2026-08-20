using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class StoneController : MonoBehaviour
{
    [Header("参照カメラ")]
    public Camera targetCamera;

    [Header("ショット設定")]
    public float powerMultiplier = 10f;
    public float maxDragDistance = 5f;
    public float minDragDistance = 0.3f;

    [Header("回転設定")]
    public float rotationPower = 5f;

    [Header("落下判定")]
    public float fallLimitY = -10f;

    [Header("重力設定")]
    public float gravityMultiplier = 2f;

    [Header("発射可能角度")]
    [Range(0f, 180f)]
    public float shotAngleLimit = 180f;

    [Header("効果音")]
    public AudioClip shotSound;

    public bool IsDragging => dragging;
    public float CurrentPower { get; private set; }
    public Vector3 CurrentDirection { get; private set; }

    private Rigidbody rb;
    private AudioSource audioSource;

    private bool dragging;

    private Vector3 dragStartMouse;
    private Vector3 dragCurrentMouse;

    private Vector3 lastShotPosition;
    private Quaternion lastShotRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        lastShotPosition = transform.position;
        lastShotRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        if (gravityMultiplier != 1f)
        {
            rb.AddForce(
                Physics.gravity *
                (gravityMultiplier - 1f),
                ForceMode.Acceleration
            );
        }
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

        // 停止補助
        if (rb.linearVelocity.magnitude < 0.05f)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void StartDrag()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
            return;

        dragging = true;

        dragStartMouse = Input.mousePosition;
        dragCurrentMouse = dragStartMouse;
    }

    void UpdateDrag()
    {
        if (targetCamera == null)
            return;

        dragCurrentMouse = Input.mousePosition;

        Vector3 mouseDelta =
            dragCurrentMouse -
            dragStartMouse;

        Vector3 cameraForward =
            targetCamera.transform.forward;

        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight =
            targetCamera.transform.right;

        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 direction =
            (-mouseDelta.y * cameraForward) +
            (-mouseDelta.x * cameraRight);

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            direction.Normalize();

            if (shotAngleLimit < 180f)
            {
                float angle =
                    Vector3.SignedAngle(
                        cameraForward,
                        direction,
                        Vector3.up
                    );

                angle =
                    Mathf.Clamp(
                        angle,
                        -shotAngleLimit,
                        shotAngleLimit
                    );

                direction =
                    Quaternion.Euler(
                        0f,
                        angle,
                        0f
                    ) *
                    cameraForward;

                direction.Normalize();
            }
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
            dragCurrentMouse -
            dragStartMouse;

        float dragDistance =
            mouseDelta.magnitude / 100f;

        if (dragDistance < minDragDistance)
        {
            CurrentPower = 0f;
            CurrentDirection = Vector3.zero;
            return;
        }

        lastShotPosition = transform.position;
        lastShotRotation = transform.rotation;

        float power =
            Mathf.Clamp(
                dragDistance,
                0f,
                maxDragDistance
            );

        Vector3 shootDirection =
            CurrentDirection.normalized;

        rb.AddForce(
            shootDirection *
            power *
            powerMultiplier,
            ForceMode.Impulse
        );

        Vector3 rotationAxis =
            Vector3.Cross(
                Vector3.up,
                shootDirection
            );

        rb.AddTorque(
            rotationAxis *
            power *
            rotationPower,
            ForceMode.Impulse
        );

        // 発射音
        if (shotSound != null)
        {
            audioSource.PlayOneShot(
                shotSound
            );
        }

        CurrentPower = 0f;
        CurrentDirection = Vector3.zero;
    }

    public void ReturnToPreviousShot()
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