using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHealth;

    [Header("Status Details")]
    public Stat_MajorGroup majorStat;
    public Stat_OffenseGroup offenseStat;
    public Stat_DefenseGroup defenseStat;

    public float GetMaxHealth()
    {
        float baseValue = maxHealth.GetValue();
        float bonusHp = majorStat.vitality.GetValue() * 5;

        return baseValue + bonusHp;
    }

    public float GetEvasion()
    {
        float baseValue = defenseStat.evasion.GetValue();
        float bonusEvasion = majorStat.agility.GetValue() * 0.5f;

        return baseValue + bonusEvasion;
    }
}
