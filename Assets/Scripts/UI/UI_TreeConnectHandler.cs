using System;
using UnityEngine;

[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType directionType;
    [Range(100f, 350f)] public float length;
}

public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();

    [SerializeField] private UI_TreeConnectDetails[] connectionDetails;
    [SerializeField] private UI_TreeConnection[] connections;

    private void OnValidate()
    {
        if (connectionDetails.Length != connections.Length)
        {
            Debug.LogWarning($"The amount of details is the same as the length of connections, Log from {gameObject.name}");
            return;
        }

        UpdateConnection();
    }

    private void UpdateConnection()
    {
        for (int i = 0; i < connectionDetails.Length; i++)
        {
            var detail = connectionDetails[i];
            var connection = connections[i];

            connection.DirectConnection(detail.directionType, detail.length);

            Vector2 targetPoisition = connection.GetChildConnectionPoint(rect);
            detail.childNode?.SetPosition(targetPoisition);
        }
    }

    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
}
