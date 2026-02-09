using System.Text;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTree;

    [SerializeField] private TextMeshProUGUI skillNameText;
    [Space]
    [SerializeField] private TextMeshProUGUI skillDescText;
    [SerializeField] private TextMeshProUGUI skillReqiText;
    [Space]
    [SerializeField] private string skillRequirementTitle = "Skill Requirements";
    [SerializeField] private string skillLocksOuttTitle = "Locks Out";
    [Space]
    [TextArea]
    [SerializeField] private string lockedSkillText = "You have taken a different path, this skill can not be unlocked.";
    [Space]
    [SerializeField] private string metConditionHexColor;
    [SerializeField] private string notMetConditionHexColor;
    [SerializeField] private string urgentHexColor;
    [SerializeField] private Color exampleColor;

    protected override void Awake()
    {
        base.Awake();

        ui.GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>();
    }

    public void ShowToolTip(bool show, RectTransform targetRect, UI_TreeNode skillNode)
    {
        base.ShowToolTip(show, targetRect);

        if (!show)
            return;

        skillNameText.text = skillNode.skillData.displayName;
        skillDescText.text = $"{skillNode.skillData.description}"; //If you want tab for text put \t in the string

        string skillLockedText = $"<color={urgentHexColor}>{lockedSkillText}</color>";
        string requirementsText = skillNode.isLocked ?
            skillLockedText :
            $"{GetRequirements(skillNode.skillData.cost, skillNode.neededNodes, skillNode.conflictNodes)}";

        skillReqiText.text = requirementsText;
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] neededNoes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"{skillRequirementTitle}");

        //I may change this into checkbox instead of changing color when met the conditions;
        string costColor = skillTree.HaveEnoughSkillPoints(skillCost) ? metConditionHexColor : notMetConditionHexColor;
        string pluralSkillCost = skillCost == 1 ? "" : "s";
        sb.AppendLine($"<color={costColor}> - {skillCost} skill point{pluralSkillCost}</color>");

        foreach (var node in neededNoes)
        {
            string neededNodeColor = node.isUnlocked ? metConditionHexColor : notMetConditionHexColor;
            sb.AppendLine($"<color={neededNodeColor}> - {node.skillData.displayName}</color>");
        }

        //Get conflict skill nodes-----------------------
        if (skillTree.SkillTreeOnePath == false)
            return sb.ToString();

        if (conflictNodes.Length <= 0)
            return sb.ToString();

        sb.AppendLine();
        sb.AppendLine($"{skillLocksOuttTitle}");
        foreach (var node in conflictNodes)
        {
            sb.AppendLine($"<color={urgentHexColor}> - {node.skillData.displayName}</color>");
        }
        //-----------------------------------------------

        return sb.ToString();
    }
}
