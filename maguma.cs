using UnityEngine;

public class MagmaLevel : MonoBehaviour
{
    [SerializeField] float amplitude = 0.5f;
    [SerializeField] float speed = 1f;

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        float height = 1 + Mathf.Sin(Time.time * speed) * amplitude;

        transform.localScale = new Vector3(
            initialScale.x,
            initialScale.y * height,
            initialScale.z
        );
    }
}