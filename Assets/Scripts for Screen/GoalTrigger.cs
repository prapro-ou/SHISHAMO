using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField]
    private GameTimer gameTimer;

    [SerializeField]
    private string resultSceneName = "ResultScene";

    private bool hasFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasFinished)
        {
            return;
        }

        if (!other.CompareTag("Stone"))
        {
            return;
        }

        hasFinished = true;

        gameTimer.StopTimer();

        ResultData.ClearTime = gameTimer.ElapsedTime;

        SceneManager.LoadScene(resultSceneName);
    }
}