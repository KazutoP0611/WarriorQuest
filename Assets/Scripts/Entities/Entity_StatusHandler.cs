using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    [Header("Shock Effect Details")]
    [SerializeField] private GameObject lightningStrikeVFX;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maxCharge = 1.0f;
    
    private ElementType currentEffect = ElementType.None;

    private CharacterEntity characterEntity;
    private Entity_Stats stats;
    private Entity_VFX entityVFX;
    private Entity_Health entityHealth;

    private Coroutine burnEffectCoroutine;
    private Coroutine chilledEffectCoroutine;
    private Coroutine shockEffectCoroutine;

    private void Awake()
    {
        characterEntity = GetComponent<CharacterEntity>();
        stats = GetComponent<Entity_Stats>();
        entityVFX = GetComponent<Entity_VFX>();
        entityHealth = GetComponent<Entity_Health>();
    }

    public void ApplyStatusEffect(ElementType element, ElementalEffectData elementalEffectData)
    {
        if (element == ElementType.Ice && CanBeApplied(ElementType.Ice))
            ApplyChillEffect(elementalEffectData.chillDuration, elementalEffectData.chillSlowMultiplier);

        if (element == ElementType.Fire && CanBeApplied(ElementType.Fire))
            ApplyBurnEffect(elementalEffectData.burnDuration, elementalEffectData.burnDamage);

        if (element == ElementType.Lightning && CanBeApplied(ElementType.Lightning))
            ApplyShockEffect(elementalEffectData.shockDuration, elementalEffectData.shockDamage, elementalEffectData.shockCharge);
    }

    public bool CanBeApplied(ElementType element)
    {
        if (element == ElementType.Lightning && currentEffect == ElementType.Lightning)
            return true;

        return currentEffect == ElementType.None;
    }

    public void ApplyBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = stats.GetElementalResistance(ElementType.Fire);
        float totalDamage = fireDamage * (1 - fireResistance);

        if (burnEffectCoroutine != null)
            StopCoroutine(burnEffectCoroutine);

        burnEffectCoroutine = StartCoroutine(BurnEffectCo(duration, totalDamage));
    }

    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        currentEffect = ElementType.Fire;

        entityVFX.PlayElementalVFX(duration, ElementType.Fire);
        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);

        float damagePerTick = totalDamage / tickCount;
        float tickIntervel = 1.0f / ticksPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            //Debug.LogWarning(damagePerTick);
            entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickIntervel);
        }

        currentEffect = ElementType.None;
    }

    public void ApplyChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = stats.GetElementalResistance(ElementType.Ice);
        float reduceDuration = duration * (1 - iceResistance);

        //Debug.LogWarning($"Reduce time debuff to {reduceDuration}");
        if (chilledEffectCoroutine != null)
            StopCoroutine(chilledEffectCoroutine);

        chilledEffectCoroutine = StartCoroutine(ChilledEffectCo(reduceDuration, slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float duration, float slowMultiplier)
    {
        currentEffect = ElementType.Ice;

        entityVFX.PlayElementalVFX(duration, ElementType.Ice);
        characterEntity.SlowDownCharacterBy(duration, slowMultiplier);

        yield return new WaitForSeconds(duration);

        currentEffect = ElementType.None;
    }

    public void ApplyShockEffect(float duration, float damage, float charge)
    {
        float lightningResistance = stats.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightningResistance);

        currentCharge += finalCharge;
        if (currentCharge >= maxCharge)
        {
            DoLightningStrike(damage);
            StopShockEffect();
            return;
        }

        if (shockEffectCoroutine != null)
            StopCoroutine(shockEffectCoroutine);

        shockEffectCoroutine = StartCoroutine(ShockEffectCo(duration));
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVFX, transform.position, Quaternion.identity);
        entityHealth.ReduceHealth(damage);
    }

    private void StopShockEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        entityVFX.StopAllVFX();
    }

    private IEnumerator ShockEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        entityVFX.PlayElementalVFX(duration, ElementType.Lightning);

        yield return new WaitForSeconds(duration);
        StopShockEffect();
    }
}
