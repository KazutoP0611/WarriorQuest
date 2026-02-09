using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI_SkillTree skillTree;

    private UI ui;
    private RectTransform rect;

    private bool m_skillOnePath;

    public bool SkillOnePath { get { return m_skillOnePath; } set { m_skillOnePath = value; } }

    [Header("Debug Details")]
    [SerializeField] private int skillCost;

    [Header("Skill Details")]
    public Skill_DataSO skillData;
    [SerializeField] private string skillName;

    [Header("Status")]
    public bool isUnlocked = false;
    [Tooltip("If you check \"Skill Tree One Path\" in UI_NodeManager, this boolean shows this skill status when player have unlocked this skill's \"Conflict Nodes\". " +
        "Meaning, this skill can not be unlocked at all after this skill's conflict nodes has been unlocked.")]
    public bool isLocked = false;

    [Header("Unlock Details")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes; //"Skill One Path"

    [Header("UI Details")]
    [SerializeField] private Image skillIcon;
    [Space]
    [SerializeField] private Color skillLockedColor;
    [SerializeField] private Color highlightedColor;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        //Debug.Log($"Rect transform in width : {rect.anchoredPosition.x} and height : {rect.anchoredPosition.y}");

        UpdateIconColor(skillLockedColor);
    }

    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = $"UI - TreeNode - {skillData.displayName}";
    }

    private bool CanBeUnlocked()
    {
        //First check if this node can be unlocked.
        if (isLocked || isUnlocked)
            return false;

        //Second check for the previous node (needed node) is unlocked.
        foreach (var node in neededNodes)
        {
            if (node.isUnlocked == false)
                return false;
        }

        //Third check for the conflict node if skill tree is on one path setting.
        if (m_skillOnePath == true)
        {
            foreach (var node in conflictNodes)
            {
                if (node.isUnlocked)
                    return false;
            }
        }

        //Forth check for require points the player has.
        if (skillTree.HaveEnoughSkillPoints(skillData.cost) == false)
            return false;

        return true;
    }

    private bool CanBeUnLockedMultiplePath()
    {
        if (isUnlocked)
            return false;

        foreach (var node in neededNodes)
        {
            if (node.isUnlocked == false)
                return false;
        }

        return true;
    }

    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        skillTree.RemoveSkillPoints(skillData.cost);

        if (m_skillOnePath == false)
            return;

        LockConflictNodes();
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            Debug.LogWarning("You have locked this node's conflict nodes! FYI for after refactor.");
        }
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;

        skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else
            Debug.LogWarning("Can not be unlocked!!");

        //if (m_skillOnePath == true)
        //{
        //    #region SkillOnePath
        //    //Use this instead if you want the skill tree to have only one path.Meaning if you unlock one path, the others path can not be use at all forever.
        //    if (CanBeUnlocked())
        //        Unlock();
        //    else
        //        Debug.LogWarning("Can not be unlocked!!");
        //    #endregion
        //}
        //else
        //{
        //    #region SkillMultiplePath
        //    if (CanBeUnLockedMultiplePath())
        //        Unlock();
        //    else
        //        Debug.LogWarning("Can not be unlocked!!");
        //    #endregion
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, this);

        if (isUnlocked == false)
            UpdateIconColor(highlightedColor);
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);

        return color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, rect);

        if (isUnlocked == false)
            UpdateIconColor(skillLockedColor);
    }
}
