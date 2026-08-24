using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class StoneUI : MonoBehaviour
{
    public StoneController stone;

    [Header("矢印設定")]
    public float minArrowLength = 0.5f;
    public float maxArrowLength = 4f;

    [Header("表示開始パワー")]
    public float arrowVisiblePower = 0.05f;

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

        if (stone.CurrentPower < arrowVisiblePower)
        {
            line.enabled = false;
            return;
        }

        Vector3 direction =
            stone.CurrentDirection;

        if (direction.magnitude < 0.01f)
        {
            line.enabled = false;
            return;
        }

        line.enabled = true;

        // 色変更
        Color arrowColor;

        if (stone.CurrentPower < 0.5f)
        {
            arrowColor = Color.Lerp(
                Color.blue,
                Color.yellow,
                stone.CurrentPower / 0.5f
            );
        }
        else
        {
            arrowColor = Color.Lerp(
                Color.yellow,
                Color.red,
                (stone.CurrentPower - 0.5f) / 0.5f
            );
        }

        line.startColor = arrowColor;
        line.endColor = arrowColor;

        // 長さ変更
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

        line.SetPosition(
            0,
            start
        );

        line.SetPosition(
            1,
            end
        );
    }
}