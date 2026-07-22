using UnityEngine;
using UnityEngine.SceneManagement;

public class CourseSelectManager : MonoBehaviour
{
    public void OnClickCourse1()
    {
        GameData.SelectedCourse = 1;
        SceneManager.LoadScene("SampleScene");
    }


    public void OnClickBack()
    {
        SceneManager.LoadScene("TitleScene");
    }
}