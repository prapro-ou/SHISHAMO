using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeLabel;



    private void Start()
    {
        ShowResult();
    }

    private void ShowResult()
    {
        timeLabel.text = $"TIME : {GameData.ClearTime:F2}";
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