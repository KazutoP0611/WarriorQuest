using System;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_ResourceGroup resources;

    [Header("Status Details")]
    public Stat_MajorGroup majorStat;
    [Tooltip("Base value 0 - 100")]
    public Stat_OffenseGroup offenseStat;
    [Tooltip("Base value 0 - 100")]
    public Stat_DefenseGroup defenseStat;

    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1.0f)
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

        isCrit = UnityEngine.Random.Range(0, 100) < totalCritChance;
        float finalDamage = isCrit ? totalDamage * totalCritPow : totalDamage;

        return finalDamage * scaleFactor;
    }

    public float GetElemetalDamage(out ElementType element, float scaleFactor = 1.0f)
    {
        float fireDamage = offenseStat.fireDamage.GetValue();
        float iceDamage = offenseStat.iceDamage.GetValue();
        float lightningDamage = offenseStat.lightningDamage.GetValue();

        float bonusElementalDamage = majorStat.intelligence.GetValue();

        float highestDamage = fireDamage;
        element = ElementType.Fire;

        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            element = ElementType.Ice;
        }

        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            element = ElementType.Lightning;
        }

        if (highestDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        float bonusFire = fireDamage == highestDamage ? 0 : fireDamage * 0.5f;
        float bonusIce = iceDamage == highestDamage ? 0 : iceDamage * 0.5f;
        float bonusLightning = lightningDamage == highestDamage ? 0 : lightningDamage * 0.5f;
        float weakerElementalDamage = bonusFire + bonusIce + bonusLightning;

        float finalDamage = highestDamage + weakerElementalDamage + bonusElementalDamage;

        return finalDamage * scaleFactor;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0;
        float bonusResistance = majorStat.intelligence.GetValue() * 0.5f;

        switch (element)
        {
            case ElementType.Fire:
                baseResistance = defenseStat.fireRes.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defenseStat.iceRes.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defenseStat.lightningRes.GetValue();
                break;
        }

        float totalResistance = baseResistance + bonusResistance;
        float resistanceCap = 75f;
        float finalResistance = Mathf.Clamp(totalResistance, 0, resistanceCap) / 100;

        return finalResistance;
    }

    public float GetMaxHealth()
    {
        float baseHealth = resources.maxHealth.GetValue();
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

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegen;

            case StatType.Strength: return majorStat.strength;
            case StatType.Agility: return majorStat.agility;
            case StatType.Intelligence: return majorStat.intelligence;
            case StatType.Vitality: return majorStat.vitality;

            case StatType.AttackSpeed: return offenseStat.attackSpeed;
            case StatType.Damage: return offenseStat.damage;
            case StatType.CritChange: return offenseStat.critChance;
            case StatType.CritPower: return offenseStat.critPower;
            case StatType.ArmorReduction: return offenseStat.armorReduction;
            case StatType.FireDamage: return offenseStat.fireDamage;
            case StatType.IceDamage: return offenseStat.iceDamage;
            case StatType.LightningDamage: return offenseStat.lightningDamage;

            case StatType.Armor: return defenseStat.armor;
            case StatType.Evasion: return defenseStat.evasion;
            case StatType.FireResistance: return defenseStat.fireRes;
            case StatType.IceResistance: return defenseStat.iceRes;
            case StatType.LightningResistance: return defenseStat.lightningRes;

            default:
                Debug.LogWarning($"Stat Type {type} is not implemented yet.");
                return null;
        }
    }
}
