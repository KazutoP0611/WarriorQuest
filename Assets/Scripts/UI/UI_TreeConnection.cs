using UnityEngine;
using UnityEngine.UI;

public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Left,
    Right,
    DownLeft,
    Down,
    DownRight
}

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectionLength;
    [SerializeField] private RectTransform childNodeConnectionPoint;

    private float GetDirectionAngle(NodeDirectionType type)
    {
        switch (type)
        {
            case NodeDirectionType.UpLeft: return 135f;
            case NodeDirectionType.Up: return 90f;
            case NodeDirectionType.UpRight: return 45f;
            case NodeDirectionType.Left: return 180f;
            case NodeDirectionType.Right: return 0f;
            case NodeDirectionType.DownLeft: return -135f;
            case NodeDirectionType.Down: return -90f;
            case NodeDirectionType.DownRight: return -45f;
            default: return 0f;
        }
    }

    public Image GetConnectionImage() => connectionLength.GetComponent<Image>();

    public void DirectConnection(NodeDirectionType direction, float length, float offset)
    {
        bool shouldBeActive = direction != NodeDirectionType.None;
        float connectionArmLength = shouldBeActive ? length : 0f;
        float angle = GetDirectionAngle(direction);

        rotationPoint.localRotation = Quaternion.Euler(0, 0, angle + offset);
        connectionLength.sizeDelta = new Vector2(connectionArmLength, connectionLength.sizeDelta.y);
    }

    //Give "localPosition" of sent "rect" that is calculated from "childNodeConnectionPoint";
    public Vector2 GetChildConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                rect.parent as RectTransform, //The reference RectTransform
                childNodeConnectionPoint.position, //Point that you want to convert
                null,
                out var localPosition //output
            );

        return localPosition;
    }
}
