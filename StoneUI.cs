using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class StoneUI : MonoBehaviour
{
    public StoneController stone;

    [Header("矢印設定")]
    public float minArrowLength = 0.5f;
    public float maxArrowLength = 4f;

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    void Update()
    {
        UpdateArrow();
    }

    void UpdateArrow()
    {
        if (!stone.IsDragging)
        {
            line.enabled = false;
            return;
        }

        line.enabled = true;

        Vector3 direction =
            stone.CurrentDirection;

        if (direction.magnitude < 0.01f)
            return;

        float length =
            Mathf.Lerp(
                minArrowLength,
                maxArrowLength,
                stone.CurrentPower
            );

        Vector3 start =
            transform.position +
            Vector3.up * 0.3f;

        Vector3 end =
            start +
            direction.normalized *
            length;

        line.positionCount = 2;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}