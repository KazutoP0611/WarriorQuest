using UnityEngine;

public class UI_DeathScreen : MonoBehaviour
{
    public void GotoCamp()
    {
        GameManager.instance.ChangeScene("Level_0", RespawnType.None);
    }

    public void LastCheckpoint()
    {
        // Have to do save check point first;
        //GameManager.instance.RestartScene();
    }

    public void SaveAndExit()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.None);
    }
}
