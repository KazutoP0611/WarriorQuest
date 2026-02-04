using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Skill_DataSO skillData;
    [SerializeField] private string skillName;

    [Header("UI Details")]
    [SerializeField] private Image skillIcon;
    [Space]
    [SerializeField] private Color skillLockedColor;
    [SerializeField] private Color highlightedColor;

    [Header("Status")]
    public bool isUnlocked = false;
    public bool isLocked = false;

    private UI ui;
    private RectTransform rect;

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
        if (CanBeUnlocked())
            Unlock();
        else
            Debug.LogWarning("Can not be unlocked!!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect);

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
