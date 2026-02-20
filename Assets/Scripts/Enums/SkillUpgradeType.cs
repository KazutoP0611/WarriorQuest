using UnityEngine;

public enum SkillUpgradeType
{
    None,

    // ------ Dash Tree ------
    Dash,
    Dash_CloneOnStart,
    Dash_CloneOnStartAndEnd,
    Dash_ShardOnStart,
    Dash_ShardOnStartAndEnd,

    // ------ Shard Tree -------
    Shard,
    Shard_MoveToEnemy,
    Shard_MultiCast,
    Shard_Teleport,
    Shard_TeleportRewindHp
}
