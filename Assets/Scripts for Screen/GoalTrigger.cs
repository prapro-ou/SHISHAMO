using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField]
    private GameTimer gameTimer;

    [SerializeField]
    private string resultSceneName = "ResultScene";

    private bool hasFinished = false;

    private void Start()
    {
        Debug.Log("GoalTriggerが起動しました");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("何かがゴールに入りました: " + other.name);

        if (hasFinished)
        {
            return;
        }

        if (!other.CompareTag("Stone"))
        {
            return;
        }

        hasFinished = true;

        // タイマー停止
        gameTimer.StopTimer();

        // クリアタイムを保存
        ResultData.ClearTime = gameTimer.ElapsedTime;

        // 今プレイしているコース名を保存
        ResultData.PreviousSceneName =
            SceneManager.GetActiveScene().name;

        // リザルト画面へ移動
        SceneManager.LoadScene(resultSceneName);
    }
}