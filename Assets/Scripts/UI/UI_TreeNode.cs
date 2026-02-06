using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;

    private bool m_skillOnePath;

    public bool SkillOnePath { get { return m_skillOnePath; } set { m_skillOnePath = value; } }

    [Header("Skill Details")]
    [SerializeField] private Skill_DataSO skillData;
    [SerializeField] private string skillName;

    [Header("Status")]
    public bool isUnlocked = false;
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
        //Debug.Log($"Rect transform in width : {rect.anchoredPosition.x} and height : {rect.anchoredPosition.y}");

        UpdateIconColor(skillLockedColor);
    }

    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        gameObject.name = $"UI - TreeNode - {skillData.displayName}";
    }

    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
            return false;

        foreach (var node in neededNodes)
        {
            if (node.isUnlocked == false)
                return false;
        }

        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
                return false;
        }

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
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;

        skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_skillOnePath)
        {
            #region SkillOnePath
            //Use this instead if you want the skill tree to have only one path.Meaning if you unlock one path, the others path can not be use at all forever.
            if (CanBeUnlocked())
                Unlock();
            else
                Debug.LogWarning("Can not be unlocked!!");
            #endregion
        }
        else
        {
            #region SkillMultiplePath
            if (CanBeUnLockedMultiplePath())
                Unlock();
            else
                Debug.LogWarning("Can not be unlocked!!");
            #endregion
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, skillData);

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
