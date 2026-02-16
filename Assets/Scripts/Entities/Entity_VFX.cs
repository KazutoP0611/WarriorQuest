using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity_VFX : MonoBehaviour
{
    private CharacterEntity entity;

    private Material originalMaterial;
    private Color defaultHitVFXColor;
    protected SpriteRenderer spriteRenderer;

    private Coroutine onDamageCoroutine;
    private Coroutine onElementalEffectCoroutine;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = 0.1f;

    [Header("On Doing Damage VFX")]
    [SerializeField] private Color hitVFXColor = Color.white;
    [SerializeField] private GameObject hitVFX;
    [Space]
    [SerializeField] private GameObject critHitVFX;

    [Header("Element Colors")]
    [SerializeField] private Color chillVFXColor = Color.cyan;
    [SerializeField] private Color burnVFXColor = Color.orangeRed;
    [SerializeField] private Color lightningVFXColor = Color.lightGoldenRodYellow;

    protected virtual void Awake()
    {
        entity = GetComponent<CharacterEntity>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        defaultHitVFXColor = hitVFXColor;
    }

    public void OnHitVFX(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critHitVFX : hitVFX;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);

        if (!isCrit)
            vfx.GetComponentInChildren<SpriteRenderer>().color = hitVFXColor;
        else
        {
            if (entity.facingDirection == -1)
                vfx.transform.Rotate(0, 180, 0);
        }
    }

    public void UpdateOnHitVFXColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.None:
                hitVFXColor = defaultHitVFXColor;
                break;
            case ElementType.Fire:
                hitVFXColor = burnVFXColor;
                break;
            case ElementType.Ice:
                hitVFXColor = chillVFXColor;
                break;
            case ElementType.Lightning:
                break;
        }
    }

    public void PlayOnDamageVFX()
    {
        if (onDamageCoroutine != null)
            StopCoroutine(onDamageCoroutine);

        onDamageCoroutine = StartCoroutine(OnDamageVFXCoroutine());
    }

    IEnumerator OnDamageVFXCoroutine()
    {
        //Change character's material to "set material" on damage for a duration, the nset it back.
        spriteRenderer.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVFXDuration);
        spriteRenderer.material = originalMaterial;
    }

    public void PlayElementalVFX(float duration, ElementType element)
    {
        if (onElementalEffectCoroutine != null)
            StopCoroutine(onElementalEffectCoroutine);

        Color effectColor = Color.white;
        switch (element)
        {
            case ElementType.Fire:
                effectColor = burnVFXColor;
                break;
            case ElementType.Ice:
                effectColor = chillVFXColor;
                break;
            case ElementType.Lightning:
                effectColor = lightningVFXColor;
                break;
        }

        onElementalEffectCoroutine = StartCoroutine(PlayElementalEffectCo(duration, effectColor));
    }

    private IEnumerator PlayElementalEffectCo(float duration, Color effectColor)
    {
        float tickInterval = 0.2f;
        float timer = 0;

        Color lighterColor = effectColor * 1.2f;
        Color darkerColor = effectColor * 0.8f;

        bool toggle = false;

        while (timer < duration)
        {
            spriteRenderer.color = toggle ? lighterColor : darkerColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timer += tickInterval;
        }

        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(duration);
    }

    public void StopAllVFX()
    {
        StopAllCoroutines();
        spriteRenderer.color = Color.white;
        spriteRenderer.material = originalMaterial;
    }
}
