using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool isRunning = false;
    private bool hasStarted = false;

    public float ElapsedTime
    {
        get { return elapsedTime; }
    }

    private void Start()
    {
        elapsedTime = 0f;
        UpdateTimerText();
    }

    private void Update()
    {
        if (!isRunning)
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        UpdateTimerText();
    }

    public void StartTimer()
    {
        if (hasStarted)
        {
            return;
        }

        hasStarted = true;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void UpdateTimerText()
    {
        timerText.text = FormatTime(elapsedTime);
    }

    public static string FormatTime(float time)
    {
        int totalSeconds = Mathf.FloorToInt(time);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return $"{minutes:00}:{seconds:00}";
    }
}