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
    [SerializeField] private Color lockedColor;
    //[SerializeField] private string lockedColorHex; //I don't know why teacher used hex?
    [Space]
    [SerializeField] private float highlightenVolume = 0.7f;
    [SerializeField] private float unHighlightenVolume = 0.25f;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectHandler = GetComponent<UI_TreeConnectHandler>();

        //UpdateIconColor(GetColorByHex(lockedColorHex));
        UpdateIconColor(lockedColor);
    }

    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = $"UI_TreeNode - {skillData.displayName}";
    }

    private void OnDisable()
    {
        if (isLocked)
            UpdateIconColor(lockedColor);
        //ToggleNodeHighlight(false);

        if (isUnlocked)
            UpdateIconColor(Color.white);
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

        //That means this can be unlocked.
        return true;
    }

    public void Refund()
    {
        if (isUnlocked)
            skillTree.AddSkillPoints(skillData.cost);

        isUnlocked = false;
        isLocked = false;

        //UpdateIconColor(GetColorByHex(lockedColorHex));
        UpdateIconColor(lockedColor);

        //At first, this can get bug, if call "UnlockBelow" alone. If you call both, there are no problems but I didn't like it.
        //Because the head node doesn't have parent image connection, so they don't have original color.
        //Now with locked color setting at connecthandler, it's fine. You can call which one you prefer.
        connectHandler.UnlockAboveConnectionImage(false);
        connectHandler.UnlockBelowConnectionImage(false);

        //reset skills in skill manager;
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

        skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeData);
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            //may be lock children of conflict node too. Right now if developer didn't set the children of conflict node in skill node's inspector.
            //the children of conflict node will still ablt to be highlighted.

            //Lock connection to parent node of the conflict node;
            node.connectHandler.UnlockAboveConnectionImage(false); // It really works!! I didn't know I can access others private variables if it is the same class. Nice!
        }
    }

    private void ToggleNodeHighlight(bool highlight)
    {
        float highlightMultiplayVolume = highlight ? highlightenVolume : unHighlightenVolume;

        Color colorToApply = Color.white * highlightMultiplayVolume;
        colorToApply.a = 1f;

        UpdateIconColor(colorToApply);
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;

        //lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else if (isLocked)
        {
            //Debug.LogWarning("Can not be unlocked!!");
            ui.skillToolTip.ShowLockedSkillEffect();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, this);

        if (isUnlocked || isLocked)
            return;

        ToggleNodeHighlight(true);

        //Color color = Color.white * highlightenVolume;
        //color.a = 1f;
        //UpdateIconColor(color);
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

        ToggleNodeHighlight(false);

        //UpdateIconColor(lastColor);
    }
}
