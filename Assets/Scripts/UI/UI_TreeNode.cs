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

    private void Awake()
    {
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
        if (isUnlocked == false)
            UpdateIconColor(highlightedColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isUnlocked == false)
            UpdateIconColor(skillLockedColor);
    }
}
