using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI_SkillTree skillTree;

    [SerializeField] private TextMeshProUGUI skillNameText;
    [Space]
    [SerializeField] private TextMeshProUGUI skillDescText;
    [SerializeField] private TextMeshProUGUI skillReqiText;
    [Space]
    [SerializeField] private GameObject skillLockOutGroup;
    [SerializeField] private TextMeshProUGUI skillLockOutTitle;
    [SerializeField] private TextMeshProUGUI skillLockText;
    [Space]
    [SerializeField] private string metConditionHexColor;
    [SerializeField] private string notMetConditionHexColor;
    [SerializeField] private string urgentHexColor;
    [SerializeField] private Color exampleColor;

    protected override void Awake()
    {
        base.Awake();

        skillTree = GetComponentInParent<UI_SkillTree>();
    }

    public void ShowToolTip(bool show, RectTransform targetRect, UI_TreeNode skillNode)
    {
        base.ShowToolTip(show, targetRect);

        if (!show)
            return;

        skillNameText.text = skillNode.skillData.displayName;
        skillDescText.text = $"{skillNode.skillData.description}"; //If you want tab for text put \t in the string

        skillReqiText.text = $"{GetRequirements(skillNode.skillData.cost, skillNode.neededNodes)}";

        if (skillTree.SkillTreeOnePath == false)
        {
            skillLockOutGroup.SetActive(false);
            //skillLockOutTitle.text = "";
            //skillLockText.text = "";
            return;
        }

        if (skillNode.conflictNodes.Length <= 0)
        {
            skillLockOutGroup.SetActive(false);
            //skillLockOutTitle.text = "";
            //skillLockText.text = "";
            return;
        }

        skillLockOutGroup.SetActive(true);
        //skillLockOutTitle.text = "Skill Locks Out";
        skillLockText.text = $"{GetLocksOutNodes(skillNode.conflictNodes)}";
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] neededNoes)
    {
        StringBuilder sb = new StringBuilder();

        //I may change this into checkbox instead of changing color when met the conditions;
        string costColor = skillTree.HaveEnoughSkillPoints(skillCost) ? metConditionHexColor : notMetConditionHexColor;
        string pluralSkillCost = skillCost == 1 ? "" : "s";
        sb.AppendLine($"<color={costColor}> - {skillCost} skill point{pluralSkillCost}</color>");

        foreach (var node in neededNoes)
        {
            string neededNodeColor = node.isUnlocked ? metConditionHexColor : notMetConditionHexColor;
            sb.AppendLine($"<color={neededNodeColor}> - {node.skillData.displayName}</color>");
        }

        return sb.ToString();
    }

    private string GetLocksOutNodes(UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var node in conflictNodes)
        {
            sb.AppendLine($"<color={urgentHexColor}> - {node.skillData.displayName}</color>");
        }

        return sb.ToString();
    }
}
