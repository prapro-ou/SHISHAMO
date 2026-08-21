using UnityEngine;
using UnityEngine.SceneManagement;

public class CourseSelectManager : MonoBehaviour
{
    public void OnClickCourse1()
    {
        GameData.SelectedCourse = 1;
        SceneManager.LoadScene("course1");
    }

    public void OnClickCourse2()
    {
        GameData.SelectedCourse = 2;
        SceneManager.LoadScene("course2");
    }

    public void OnClickCourse3()
    {
        GameData.SelectedCourse = 3;
        SceneManager.LoadScene("course3");
    }

    public void OnClickCourse4()
    {
        GameData.SelectedCourse = 4;
        SceneManager.LoadScene("course4");
    }

    public void OnClickCourse5()
    {
        GameData.SelectedCourse = 5;
        SceneManager.LoadScene("course5");
    }

    public void OnClickBack()
    {
        SceneManager.LoadScene("TitleScene");
    }
}