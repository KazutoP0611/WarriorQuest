using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillDescText;
    [SerializeField] private TextMeshProUGUI skillReqiText;

    //public override void ShowToolTip(bool show, RectTransform targetRect)
    //{
    //    base.ShowToolTip(show, targetRect);
    //}

    public void ShowToolTip(bool show, RectTransform targetRect, Skill_DataSO skillData)
    {
        base.ShowToolTip(show, targetRect);

        if (!show)
            return;

        skillNameText.text = skillData.displayName;
        skillDescText.text = skillData.description; //If you want tab for text put \t in the string
        skillReqiText.text = " - " + skillData.cost + string.Format(" skill point{0}", skillData.cost > 1 ? "s" : "");
    }
}
