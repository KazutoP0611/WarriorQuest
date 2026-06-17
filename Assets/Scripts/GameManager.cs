using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveble
{
    public static GameManager instance;

    private string lastScenePlayed;
    private Vector3 lastDeathPosition;

    [SerializeField] private float sceneFadeDuration = 1.0f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetLastPlayerPosition(Vector3 position) => lastDeathPosition = position;

    public void ContinuePlay()
    {
        ChangeScene("Level_0", RespawnType.None);
    }

    public void RestartScene()
    {
        //SaveManager.instance.SaveGame();

        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, RespawnType.None);
    }

    public void ChangeScene(string sceneName, RespawnType respawnType)
    {
        // SaveManager.instance.SaveGame();
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCo(sceneName));
    }

    private IEnumerator ChangeSceneCo(string sceneName)
    {
        // Fade in
        UI_FadeScreen fadeScreen = GetFadeScreen();
        fadeScreen.FadeIn(sceneFadeDuration);

        yield return fadeScreen.fadeCoroutine;

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(0.2f);

        // Fade out
    }

    private UI_FadeScreen GetFadeScreen()
    {
        if (UI.instance != null)
            return UI.instance.fadeScreenUI;
        else
            return FindFirstObjectByType<UI_FadeScreen>();
    }

    //private Vector3 GetNewPlayerPosition(RespawnType respawnType)
    //{
    //    if (respawnType == RespawnType.None)
    //    return Vector3.zero;
    //}

    //private Vector3 GetWaypointPosition(RespawnType respawnType)
    //{
    //    var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

    //    foreach (var waypoint in waypoints)
    //    {
    //    }
    //}

    //public void LoadData(GameData data)
    //{
    //    lastScenePlayed = data.lastScenePlayed;
    //}

    //public void SaveData(ref GameData data)
    //{
    //    string currentScene = SceneManager.GetActiveScene().name;

    //    if (currentScene == "MainMenu")
    //        return;

    //    data.lastScenePlayed = currentScene;
    //}
}
