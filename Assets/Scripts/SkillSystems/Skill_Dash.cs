using UnityEngine;

public class Skill_Dash : Skill_Base
{
    public void OnStartEffect()
    {
        if (IsSkillUnlocked(SkillUpgradeType.Dash_CloneOnStart) || IsSkillUnlocked(SkillUpgradeType.Dash_CloneOnStartAndEnd))
            CreateClone();

        if (IsSkillUnlocked(SkillUpgradeType.Dash_ShardOnStart) || IsSkillUnlocked(SkillUpgradeType.Dash_ShardOnStartAndEnd))
            CreateShard();
    }

    public void OnEndEffect()
    {
        if (IsSkillUnlocked(SkillUpgradeType.Dash_CloneOnStartAndEnd))
            CreateClone();

        if (IsSkillUnlocked(SkillUpgradeType.Dash_ShardOnStartAndEnd))
            CreateShard();
    }

    private void CreateShard()
    {
        //Debug.Log("Activate create shard when dash!");
        skillmanager.shard.CreateRawShard();
    }

    private void CreateClone()
    {
        Debug.Log("Activate create time echo when dash!");
    }
}
