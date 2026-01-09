using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private CharacterEntity characterEntity;
    private ElementType currentEffect = ElementType.None;

    private Coroutine elementalEffectCoroutine;
    private Entity_Stats stats;
    private Entity_VFX entityVFX;

    private void Awake()
    {
        stats = GetComponent<Entity_Stats>();
        characterEntity = GetComponent<CharacterEntity>();
        entityVFX = GetComponent<Entity_VFX>();
    }

    public bool CanBeApplied(ElementType element)
    {
        return currentEffect == ElementType.None;
    }

    public void AppliedChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = stats.GetElementalResistance(ElementType.Ice);
        float reduceDuration = duration * (1 - iceResistance);

        Debug.LogWarning($"Reduce time debuff to {reduceDuration}");
        if (elementalEffectCoroutine != null)
            StopCoroutine(elementalEffectCoroutine);

        elementalEffectCoroutine = StartCoroutine(ChilledEffectCo(reduceDuration, slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float duration, float slowMultiplier)
    {
        characterEntity.SlowDownCharacterBy(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        entityVFX.PlayElementalVFX(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);

        currentEffect = ElementType.None;
    }
}
