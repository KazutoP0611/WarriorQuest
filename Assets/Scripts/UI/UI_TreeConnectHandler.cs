using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler handlerChildNode;
    public NodeDirectionType directionType;
    [Range(100f, 350f)] public float length;
    [Range(-25f, 25f)] public float rotation;
}

public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();

    private Image connectionFromParent;
    private Color originalColor;

    [SerializeField] private UI_TreeConnectDetails[] connectionDetails;
    [SerializeField] private UI_TreeConnection[] connections;
    [Space]
    [SerializeField] private Color connectionPathOpenColor;

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
        if (connectionFromParent != null)
            originalColor = connectionFromParent.color;
    }

    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;

    public void SetConnectionImage(Image image) => connectionFromParent = image;

    private void UpdateConnection()
    {
        for (int i = 0; i < connectionDetails.Length; i++)
        {
            var detail = connectionDetails[i];
            var connection = connections[i];

            connection.DirectConnection(detail.directionType, detail.length, detail.rotation);
            Image connectionImage = connection.GetConnectionImage();

            Vector2 targetPoisition = connection.GetChildConnectionPoint(rect);

            if (detail.handlerChildNode == null)
                continue;

            detail.handlerChildNode?.SetPosition(targetPoisition);
            detail.handlerChildNode?.SetConnectionImage(connectionImage);
            detail.handlerChildNode?.transform.SetAsLastSibling();
            //detail.handlerChildNode?.UpdateConnection(); //This may causes problems teacher mentioned in video,
                                                         //about doing this and you accidentally set children and
                                                         //parent to update each other, they can shut down the application.
        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnection();

        foreach (var childNodeDetail in connectionDetails)
        {
            if (childNodeDetail.handlerChildNode == null)
                continue;

            childNodeDetail.handlerChildNode.UpdateConnection();
        }
    }

    public void UnlockBelowConnectionImage(bool unlocked)
    {
        foreach (var connection in connections)
            connection.GetConnectionImage().color = unlocked ? connectionPathOpenColor : originalColor;
    }

    public void UnlockAboveConnectionImage(bool unlocked)
    {
        if (connectionFromParent == null)
            return;

        connectionFromParent.color = unlocked ? Color.white : originalColor;
    }
}
