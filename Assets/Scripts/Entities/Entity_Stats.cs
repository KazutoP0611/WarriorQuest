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
        float bonusHp = majorStat.vitality.GetValue() * 5;

        return maxHealth.GetValue() + bonusHp;
    }
}
