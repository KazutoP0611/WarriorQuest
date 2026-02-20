using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill Data - ")]
public class Skill_DataSO : ScriptableObject
{
    public bool unlockByDefault;
    public int cost;
    public SkillType skillType;
    public UpgradeData upgradeData;

    [Header("Skill Details")]
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;
}

[Serializable]
public class UpgradeData
{
    public SkillUpgradeType skillUpgradeType;
    public float cooldownTime;
    public DamageScaleData damageScaleData;
}
