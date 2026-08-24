using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("距離設定")]
    public float defaultDistance = 8f;
    public float minDistance = 3f;
    public float maxDistance = 15f;

    [Header("高さ設定")]
    public float minHeight = 2f;
    public float maxHeight = 4f;

    [Header("回転設定")]
    public float mouseSensitivity = 3f;

    [Header("ズーム設定")]
    public float zoomSpeed = 2f;
    public float zoomSmoothSpeed = 8f;

    [Header("デッドゾーン")]
    public float horizontalDeadZone = 2f;
    public float verticalDeadZone = 2f;

    [Header("視線追従速度")]
    public float lookFollowSpeed = 3f;

    [Header("注視点高さ")]
    public float lookHeight = 0.5f;

    private float yaw;

    private float currentDistance;
    private float targetDistance;

    private float currentLookX;
    private float currentLookY;

    private Rigidbody targetRb;

    void Start()
    {
        currentDistance = defaultDistance;
        targetDistance = defaultDistance;

        yaw = transform.eulerAngles.y;

        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody>();

            currentLookX = target.position.x;
            currentLookY = target.position.y + lookHeight;
        }
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        if (Input.GetMouseButton(1))
        {
            yaw +=
                Input.GetAxis("Mouse X") *
                mouseSensitivity;
        }

        float wheel =
            Input.mouseScrollDelta.y;

        if (wheel != 0f)
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

        bool isMoving = false;

        if (targetRb != null)
        {
            isMoving =
                targetRb.linearVelocity.magnitude >
                0.1f;
        }

        float targetLookX =
            target.position.x;

        float targetLookY =
            target.position.y +
            lookHeight;

        if (!isMoving)
        {
            currentLookX =
                Mathf.Lerp(
                    currentLookX,
                    targetLookX,
                    lookFollowSpeed *
                    Time.deltaTime
                );

            currentLookY =
                Mathf.Lerp(
                    currentLookY,
                    targetLookY,
                    lookFollowSpeed *
                    Time.deltaTime
                );
        }
        else
        {
            float diffX =
                targetLookX -
                currentLookX;

            if (Mathf.Abs(diffX) >
                horizontalDeadZone)
            {
                currentLookX =
                    Mathf.Lerp(
                        currentLookX,
                        targetLookX,
                        lookFollowSpeed *
                        Time.deltaTime
                    );
            }

            float diffY =
                targetLookY -
                currentLookY;

            if (Mathf.Abs(diffY) >
                verticalDeadZone)
            {
                currentLookY =
                    Mathf.Lerp(
                        currentLookY,
                        targetLookY,
                        lookFollowSpeed *
                        Time.deltaTime
                    );
            }
        }

        Vector3 focusPoint =
            new Vector3(
                currentLookX,
                currentLookY,
                target.position.z
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

        transform.position =
            focusPoint +
            offset;

        transform.LookAt(
            focusPoint
        );
    }
}