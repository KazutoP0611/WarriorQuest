using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHealth;

    [Header("Status Details")]
    public Stat_MajorGroup majorStat;
    public Stat_OffenseGroup offenseStat;
    public Stat_DefenseGroup defenseStat;

    public float GetPhysicalDamage(out bool isCrit)
    {
        float baseDamage = offenseStat.damage.GetValue();
        float bonusDamage = majorStat.strength.GetValue();
        float totalDamage = baseDamage + bonusDamage;
        
        float baseCritChance = offenseStat.critChance.GetValue();
        float bonusCritChance = majorStat.agility.GetValue() * 0.3f;
        float totalCritChance = baseCritChance + bonusCritChance;

        float baseCritPow = offenseStat.critPower.GetValue();
        float bonusCritPow = majorStat.strength.GetValue() * 0.5f;
        float totalCritPow = (baseCritPow + bonusCritPow) / 100;

        isCrit = Random.Range(0, 100) < totalCritChance;
        float finalDamage = isCrit ? totalDamage * totalCritPow : totalDamage;

        return finalDamage;
    }

    public float GetElemetalDamage()
    {
        float fireDamage = offenseStat.fireDamage.GetValue();
        float iceDamage = offenseStat.iceDamage.GetValue();
        float lightningDamage = offenseStat.lightningDamage.GetValue();

        float bonusElementalDamage = majorStat.intelligence.GetValue();

        float highestDamage = fireDamage;
        if (iceDamage > highestDamage)
            highestDamage = iceDamage;

        if (lightningDamage > highestDamage)
            highestDamage = lightningDamage;

        if (highestDamage <= 0)
            return 0;

        float bonusFire = fireDamage == highestDamage ? 0 : fireDamage * 0.5f;
        float bonusIce = iceDamage == highestDamage ? 0 : iceDamage * 0.5f;
        float bonusLightning = lightningDamage == highestDamage ? 0 : lightningDamage * 0.5f;
        float weakerElementalDamage = bonusFire + bonusIce + bonusLightning;

        float finalDamage = highestDamage + weakerElementalDamage + bonusElementalDamage;

        return finalDamage;
    }

    public float GetMaxHealth()
    {
        float baseHealth = maxHealth.GetValue();
        float bonusHealth = majorStat.vitality.GetValue() * 5;
        float totalMaxHealth = baseHealth + bonusHealth;

        return totalMaxHealth;
    }

    public float GetEvasion()
    {
        float baseEvade = defenseStat.evasion.GetValue();
        float bonusEvade = majorStat.agility.GetValue() * 0.5f;

        float totalEvasion = baseEvade + bonusEvade;

        float evasionCap = 80f;
        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }

    public float GetArmorMitigation(float attackerArmorReduction)
    {
        float baseArmor = defenseStat.armor.GetValue();
        float bonusArmor = majorStat.vitality.GetValue();
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Mathf.Clamp01(1 - attackerArmorReduction);
        float effectiveArmor = totalArmor * reductionMultiplier;

        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = 0.85f;
        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    public float GetArmorReduction()
    {
        float finalReduction = offenseStat.armorReduction.GetValue() / 100;
        return finalReduction;
    }
}
