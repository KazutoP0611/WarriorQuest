using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler handlerChildNode;
    public NodeDirectionType directionType;
    [Range(100f, 350f)] public float length;
}

public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();

    private Image connectionImage;
    private Color originalColor;

    [SerializeField] private UI_TreeConnectDetails[] connectionDetails;
    [SerializeField] private UI_TreeConnection[] connections;

    private void OnValidate()
    {
        if (connectionDetails.Length <= 0)
            return;

        if (connectionDetails.Length != connections.Length)
        {
            Debug.LogWarning($"The amount of details is the same as the length of connections, Log from {gameObject.name}");
            return;
        }

        UpdateConnection();
    }

    private void Awake()
    {
        if (connectionImage != null)
            originalColor = connectionImage.color;
    }

    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;

    public void SetConnectionImage(Image image) => connectionImage = image;

    private void UpdateConnection()
    {
        for (int i = 0; i < connectionDetails.Length; i++)
        {
            var detail = connectionDetails[i];
            var connection = connections[i];

            connection.DirectConnection(detail.directionType, detail.length);
            Image connectionImage = connection.GetConnectionImage();

            Vector2 targetPoisition = connection.GetChildConnectionPoint(rect);
            detail.handlerChildNode?.SetPosition(targetPoisition);
            detail.handlerChildNode?.SetConnectionImage(connectionImage);
        }
    }

    public void UnlockBelowConnectionImage(bool unlocked)
    {
        foreach (var connection in connections)
            connection.GetConnectionImage().color = unlocked ? Color.white : originalColor;
    }

    public void UnlockAboveConnectionImage(bool unlocked)
    {
        if (connectionImage == null)
            return;

        connectionImage.color = unlocked ? Color.white : originalColor;
    }
}
