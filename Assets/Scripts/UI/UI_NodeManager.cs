using System.Collections.Generic;
using UnityEngine;

public class UI_NodeManager : MonoBehaviour
{
    //private List<UI_TreeNode> nodes = new List<UI_TreeNode>();

    [SerializeField] private bool skillTreeOnePath = true;

    private void Awake()
    {
        UI_TreeNode[] allSkillTreeNode = GetComponentsInChildren<UI_TreeNode>();

        Debug.LogWarning($"There are {allSkillTreeNode.Length} in this game.");

        foreach (var treeNode in allSkillTreeNode)
        {
            treeNode.SkillOnePath = skillTreeOnePath;
        }
    }
}
