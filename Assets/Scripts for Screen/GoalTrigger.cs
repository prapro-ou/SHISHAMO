using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField]
    private GameTimer gameTimer;

    [SerializeField]
    private string resultSceneName = "ResultScene";

    [SerializeField]
    private AudioClip goalSound;

    private bool hasFinished = false;

    private void Start()
    {
        Debug.Log("GoalTriggerが起動しました");
    }

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

        Debug.Log("ゴールしました");

        // タイマー停止
        gameTimer.StopTimer();

        // クリアタイムを保存
        ResultData.ClearTime = gameTimer.ElapsedTime;

        // 現在プレイしているコースを保存
        ResultData.PreviousSceneName =
            SceneManager.GetActiveScene().name;

        // ゴールSEを鳴らす
        if (goalSound != null)
        {
            AudioSource.PlayClipAtPoint(
                goalSound,
                Camera.main.transform.position
            );
        }

        // 1秒後にResultSceneへ
        StartCoroutine(GoToResult());
    }

    private IEnumerator GoToResult()
    {
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene(resultSceneName);
    }
}