using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private Vector2 offset = new Vector2(300, 20);

    private Canvas canvas;
    private RectTransform rect;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rect = GetComponent<RectTransform>();
    }

    public void ShowToolTip(bool show, RectTransform targetRect)
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
        float screenCenterX = Screen.width / 2.0f;
        float screenTop = Screen.height;

        float screenBottom = 0;
        Vector2 targetPosition = targetRect.position;
        float scale = canvas.GetComponent<RectTransform>().localScale.x;

        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - (offset.x * scale) : targetPosition.x + (offset.x * scale);

        float verticalHalf = (rect.sizeDelta.y / 2.0f) * scale;
        float topY = targetPosition.y + verticalHalf;
        float bottomY = targetPosition.y - verticalHalf;

        if (topY > screenTop)
            targetPosition.y = screenTop - verticalHalf - (offset.y * scale);
        else if (bottomY < screenBottom)
            targetPosition.y = screenBottom + verticalHalf + (offset.y * scale);

        rect.position = targetPosition;
    }
}
