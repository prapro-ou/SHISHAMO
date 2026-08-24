using UnityEngine;

public class GroundMover : MonoBehaviour
{
    [Header("移動量")]
    public Vector3 moveAmount =
        new Vector3(2f, 2f, 0f);

    [Header("移動速度")]
    public float moveSpeed = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float t =
            Mathf.Sin(Time.time * moveSpeed);

        transform.position =
            startPosition +
            new Vector3(
                moveAmount.x * t,
                moveAmount.y * t,
                moveAmount.z * t
            );
    }
}