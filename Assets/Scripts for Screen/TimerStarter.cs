using UnityEngine;

public class TimerStarter : MonoBehaviour
{
    private Rigidbody rb;
    private GameTimer timer;

    private bool started = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timer = FindObjectOfType<GameTimer>();
    }

    void Update()
    {
        if (started) return;

        if (rb.linearVelocity.magnitude > 0.1f)
        {
            started = true;
            timer.StartTimer();
        }
    }
}
