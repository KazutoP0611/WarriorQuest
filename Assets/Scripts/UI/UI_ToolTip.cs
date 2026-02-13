using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private Vector2 offset = new Vector2(300, 20);

    private Canvas canvas;
    private RectTransform rect;

    protected virtual void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rect = GetComponent<RectTransform>();
    }

    public virtual void ShowToolTip(bool show, RectTransform targetRect)
    {
        if (!show)
        {
            rect.position = new Vector2(9999, 9999);
            return;
        }

        UpdatePosition(targetRect);
    }

    private void UpdatePosition(RectTransform targetRect)
    {
        float screenCenterX = Screen.width / 2.0f;//Get gameplay screen width due to "Game" window size in Unity;
        float screenTop = Screen.height;//Get gameplay screen height due to "Game" window size in Unity;
        float screenBottom = 0;

        Vector2 targetPosition = targetRect.position;//Get "World Position" of SkillNode
        float scale = canvas.GetComponent<RectTransform>().localScale.x;

        //I have to multiply with Canvas's Scale to make the offset distance fit or else it will go all over the place;
        //Calculate x position for tool tip window;
        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - (offset.x * scale) : targetPosition.x + (offset.x * scale);
        //-----------------------------------------

        //Calculate y position for tool tip window;
        float verticalHalf = (rect.sizeDelta.y / 2.0f) * scale;
        float topY = targetPosition.y + verticalHalf;
        float bottomY = targetPosition.y - verticalHalf;

        if (topY > screenTop)
            targetPosition.y = screenTop - verticalHalf - (offset.y * scale);
        else if (bottomY < screenBottom)
            targetPosition.y = screenBottom + verticalHalf + (offset.y * scale);
        //-----------------------------------------

        rect.position = targetPosition;
    }

    protected string GetColoredText(string hexColor, string text)
    {
        return $"<color={hexColor}>{text}</color>";
    }
}
