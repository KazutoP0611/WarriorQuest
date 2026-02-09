using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI_SkillTree skillTree;

    private UI ui;
    private RectTransform rect;
    private UI_TreeConnectHandler connectHandler;

    private bool m_skillOnePath;
    private Color lastColor;

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
    [SerializeField] private float highlightenVolume = 0.7f;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectHandler = GetComponent<UI_TreeConnectHandler>();
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

    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);

        skillTree.RemoveSkillPoints(skillData.cost);
        connectHandler.UnlockBelowConnectionImage(true); //Unlock connection to children nodes;
        connectHandler.UnlockAboveConnectionImage(true); //Unlock connection to parent nodes;

        if (m_skillOnePath == true)
            LockConflictNodes();
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            //Lock connection to parent node of the conflict node;
            node.connectHandler.UnlockAboveConnectionImage(false); // It really works!! I didn't know I can access others private variables if it is the same class. Nice!
        }
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;

        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else
            Debug.LogWarning("Can not be unlocked!!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, this);

        if (isUnlocked || isLocked)
            return;

        Color color = Color.white * highlightenVolume;
        color.a = 1f;
        UpdateIconColor(color);
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);

        return color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, rect);

        if (isUnlocked || isLocked)
            return;

        UpdateIconColor(lastColor);
    }
}
