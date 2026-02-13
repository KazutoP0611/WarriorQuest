using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTree;

    private Coroutine TextBlinkEffectCoroutine;

    [Header("Tooltips Details")]
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

    [Header("Blink Locked Skill Details")]
    //This settings are for skill tree one path setting. If Skill Tree is in one path setting, and a skill is unlocked.
    //When player click on the conflict skills of unlocked skill, The requirement text field will blink follow these settings.
    [Tooltip("Interval time between blinks")]
    [SerializeField] private float blinkInterval = 0.15f;
    [Tooltip("Amount of blink times")]
    [SerializeField] private int defaultBlinkCount = 3;

    protected override void Awake()
    {
        base.Awake();

        ui = GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);
    }

    public void ShowToolTip(bool show, RectTransform targetRect, UI_TreeNode skillNode)
    {
        base.ShowToolTip(show, targetRect);

        if (!show)
            return;

        skillNameText.text = skillNode.skillData.displayName;
        skillDescText.text = $"{skillNode.skillData.description}"; //If you want tab for text put \t in the string

        string skillLockedText = GetColoredText(urgentHexColor, lockedSkillText);

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

        string text = $" - {skillCost} skill point{pluralSkillCost}";
        string finalText = GetColoredText(costColor, text);
        sb.AppendLine(finalText);

        foreach (var node in neededNoes)
        {
            string neededNodeColor = node.isUnlocked ? metConditionHexColor : notMetConditionHexColor;

            string neededNodeDisplayName = $" - {node.skillData.displayName}";
            string finalNeededNodeDisplayName = GetColoredText(neededNodeColor, neededNodeDisplayName);
            sb.AppendLine(finalNeededNodeDisplayName);
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
            string conflictSkillName = $" - {node.skillData.displayName}";
            string finalConflictSkillName = GetColoredText(urgentHexColor, conflictSkillName);
            sb.AppendLine(finalConflictSkillName);
        }
        //-----------------------------------------------

        return sb.ToString();
    }

    public void ShowLockedSkillEffect()
    {
        if (TextBlinkEffectCoroutine != null)
            StopCoroutine(TextBlinkEffectCoroutine);

        TextBlinkEffectCoroutine = StartCoroutine(TextBlinkEffectCo(skillReqiText, blinkInterval, defaultBlinkCount));
    }

    private IEnumerator TextBlinkEffectCo(TextMeshProUGUI text, float blinkInterval, int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.text = GetColoredText(notMetConditionHexColor, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);

            text.text = GetColoredText(urgentHexColor, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
