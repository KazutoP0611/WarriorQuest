using System;

[Serializable]
public class AttackData
{
    public float physicalDamage;
    public float elementalDamage;
    public bool isCrit;
    public ElementType element;
    public ElementalEffectData elementalEffectData;

    public AttackData(Entity_Stats stats, DamageScaleData damageScaleData)
    {
        physicalDamage = stats.GetPhysicalDamage(out isCrit, damageScaleData.physicalDamageScale);
        elementalDamage = stats.GetElemetalDamage(out element, damageScaleData.elementalDamageScale);

        elementalEffectData = new ElementalEffectData(stats, damageScaleData);
    }
}
