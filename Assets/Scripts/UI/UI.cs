using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_ToolTip skillToolTip;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UI_ToolTip>();
    }
}
