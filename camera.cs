using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("カメラ設定")]
    public float distance = 8f;
    public float height = 4f;

    [Header("回転速度")]
    public float mouseSensitivity = 3f;

    [Header("ズーム設定")]
    public float zoomSpeed = 2f;
    public float minDistance = 3f;
    public float maxDistance = 15f;

    private float yaw;

    void Start()
    {
        yaw = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // マウスホイールでズーム
        float wheel = Input.mouseScrollDelta.y;

        if (wheel != 0)
        {
            distance -= wheel * zoomSpeed;

            distance = Mathf.Clamp(
                distance,
                minDistance,
                maxDistance
            );
        }

        // 右クリック中のみ回転
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");

            yaw += mouseX * mouseSensitivity;
        }

        Quaternion rotation =
            Quaternion.Euler(0f, yaw, 0f);

        Vector3 offset =
            rotation * new Vector3(0f, height, -distance);

        transform.position =
            target.position + offset;

        transform.LookAt(
            target.position + Vector3.up * 0.5f
        );
    }
}