using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    private string lastScenePlayed;

    public void Play()
    {
        GameManager.instance.ContinuePlay();
    }

    public void Option()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
