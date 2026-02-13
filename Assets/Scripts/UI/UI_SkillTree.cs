using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    public bool SkillTreeOnePath { get => skillTreeOnePath; private set {  skillTreeOnePath = value; } }

    [SerializeField] private int skillPoints;

    [Tooltip("\"Skill Tree One Path\" defines skill tree unlock path. Skill Tree One Path makes player able to unlock only one path, " +
        "if that skill has multiple paths. Check this boolean and player will not able to unlock other paths ever. " +
        "Uncheck this and player can unlock any path after they have unlocked previous skills, although they have unlocked other paths,")]
    [SerializeField] private bool skillTreeOnePath = true;

    [SerializeField] private UI_TreeConnectHandler[] parentNodes;

    private void Awake()
    {
        UI_TreeNode[] allSkillTreeNode = GetComponentsInChildren<UI_TreeNode>();

        string debugMessage = string.Format("There are {0} skill{1} in this game.", allSkillTreeNode.Length, allSkillTreeNode.Length > 1 ? "s" : "");
        Debug.LogWarning(debugMessage);

        foreach (var treeNode in allSkillTreeNode)
        {
            treeNode.SkillOnePath = skillTreeOnePath;
        }
    }

    private void Start()
    {
        UpdateAllParentNodesConnections();
    }

    public bool HaveEnoughSkillPoints(int cost) => skillPoints >= cost;

    public void AddSkillPoints(int points) => skillPoints = skillPoints + points;

    public void RemoveSkillPoints(int cost) => skillPoints = skillPoints - cost;

    [ContextMenu("Reset All Skills")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] allSkillNodes = GetComponentsInChildren<UI_TreeNode>();
        foreach(var node in allSkillNodes)
        {
            node.Refund();
        }
    }

    [ContextMenu("Update All Connections")]
    public void UpdateAllParentNodesConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}
