using UnityEngine;

public class UI : MonoBehaviour
{
    private bool skillTreeEnabled;

    public UI_SkillTree skillTree;
    public UI_SkillToolTip skillToolTip;

    private void Awake()
    {
        skillTree = GetComponentInChildren<UI_SkillTree>(true); //Added "true" in parameter to get component in children even the game object is disable.
        skillTreeEnabled = skillTree.gameObject.activeSelf;

        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
    }

    public void ToggleSkillTreeUI()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);

        if (skillTreeEnabled == false)
            skillToolTip.ShowToolTip(false, null);
    }
}
