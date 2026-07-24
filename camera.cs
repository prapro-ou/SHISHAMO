using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("距離設定")]
    public float defaultDistance = 8f;
    public float minDistance = 3f;
    public float maxDistance = 15f;

    [Header("高さ設定")]
    public float minHeight = 1.5f;
    public float maxHeight = 5f;

    [Header("ズーム設定")]
    public float zoomSpeed = 2f;
    public float zoomSmoothSpeed = 8f;

    [Header("回転設定")]
    public float mouseSensitivity = 3f;

    [Header("追従速度")]
    public float normalFollowSpeed = 12f;
    public float movingFollowSpeed = 2f;

    [Header("追従速度復帰")]
    public float followRestoreSpeed = 2f;

    [Header("発射後ディレイ")]
    public float followDelay = 0.7f;

    [Header("カメラ操作解禁")]
    public float cameraControlDelay = 0.7f;

    [Header("視線設定")]
    public float lookHeight = 0.5f;

    [Header("上下デッドゾーン")]
    public float verticalDeadZone = 2f;

    [Header("上下追従速度")]
    public float verticalFollowSpeed = 3f;

    private Rigidbody targetRb;

    private float yaw;

    private float currentDistance;
    private float targetDistance;

    private float moveTimer;
    private float currentFollowSpeed;

    private float currentLookY;

    void Start()
    {
        yaw = transform.eulerAngles.y;

        currentDistance = defaultDistance;
        targetDistance = defaultDistance;

        currentFollowSpeed = normalFollowSpeed;

        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody>();

            currentLookY =
                target.position.y + lookHeight;
        }
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        float speed = 0f;

        if (targetRb != null)
        {
            speed = targetRb.linearVelocity.magnitude;
        }

        bool isMoving =
            speed > 0.1f;

        if (isMoving)
        {
            moveTimer += Time.deltaTime;

            currentFollowSpeed =
                movingFollowSpeed;
        }
        else
        {
            moveTimer = 0f;

            currentFollowSpeed =
                Mathf.Lerp(
                    currentFollowSpeed,
                    normalFollowSpeed,
                    followRestoreSpeed *
                    Time.deltaTime
                );
        }

        // ズーム
        float wheel =
            Input.mouseScrollDelta.y;

        if (wheel != 0)
        {
            targetDistance -=
                wheel * zoomSpeed;

            targetDistance =
                Mathf.Clamp(
                    targetDistance,
                    minDistance,
                    maxDistance
                );
        }

        currentDistance =
            Mathf.Lerp(
                currentDistance,
                targetDistance,
                zoomSmoothSpeed *
                Time.deltaTime
            );

        // カメラ回転
        bool canRotate =
            !isMoving;

        if (isMoving)
        {
            canRotate =
                moveTimer >
                cameraControlDelay;
        }

        if (canRotate &&
            Input.GetMouseButton(1))
        {
            float mouseX =
                Input.GetAxis("Mouse X");

            yaw +=
                mouseX *
                mouseSensitivity;
        }

        // 距離によって高さ調整
        float currentHeight =
            Mathf.Lerp(
                minHeight,
                maxHeight,
                Mathf.InverseLerp(
                    minDistance,
                    maxDistance,
                    currentDistance
                )
            );

        Quaternion rotation =
            Quaternion.Euler(
                0f,
                yaw,
                0f
            );

        Vector3 offset =
            rotation *
            new Vector3(
                0f,
                currentHeight,
                -currentDistance
            );

        Vector3 targetPosition =
            target.position +
            offset;

        bool canFollow =
            !isMoving;

        if (isMoving)
        {
            canFollow =
                moveTimer >
                followDelay;
        }

        if (canFollow)
        {
            transform.position =
                Vector3.Lerp(
                    transform.position,
                    targetPosition,
                    currentFollowSpeed *
                    Time.deltaTime
                );
        }

        // 上下デッドゾーン
        float targetLookY =
            target.position.y + lookHeight;

        float diff =
            targetLookY - currentLookY;

        if (Mathf.Abs(diff) >
            verticalDeadZone)
        {
            currentLookY =
                Mathf.Lerp(
                    currentLookY,
                    targetLookY,
                    verticalFollowSpeed *
                    Time.deltaTime
                );
        }

        Vector3 lookPoint =
            new Vector3(
                target.position.x,
                currentLookY,
                target.position.z
            );

        transform.LookAt(
            lookPoint
        );
    }
}
