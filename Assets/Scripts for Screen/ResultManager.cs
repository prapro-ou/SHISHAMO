using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI clearTimeText;



    private void Start()
    {
        ShowResult();
    }

    private void ShowResult()
    {
        clearTimeText.text =
            "Clear Time " + GameTimer.FormatTime(ResultData.ClearTime);
    }

    public void OnClickRetry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnClickCourseSelect()
    {
        SceneManager.LoadScene("CourseSelectScene");
    }
}