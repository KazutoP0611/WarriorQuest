using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private ElementType currentEffect = ElementType.None;

    private CharacterEntity characterEntity;
    private Entity_Stats stats;
    private Entity_VFX entityVFX;
    private Entity_Health entityHealth;

    private Coroutine burnEffectCoroutine;
    private Coroutine chilledEffectCoroutine;

    private void Awake()
    {
        characterEntity = GetComponent<CharacterEntity>();
        stats = GetComponent<Entity_Stats>();
        entityVFX = GetComponent<Entity_VFX>();
        entityHealth = GetComponent<Entity_Health>();
    }

    public bool CanBeApplied(ElementType element)
    {
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
            entityHealth.ReduceHP(damagePerTick);
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
}
