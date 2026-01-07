using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity_VFX : MonoBehaviour
{
    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = 0.1f;

    [Header("On Doing Damage VFX")]
    [SerializeField] private Color doDamageColor = Color.white;
    [SerializeField] private GameObject hitVFX;
    [Space]
    [SerializeField] private GameObject critHitVFX;

    private CharacterEntity entity;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine onDamageCoroutine;

    protected virtual void Awake()
    {
        entity = GetComponent<CharacterEntity>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    public void OnHitVFX(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critHitVFX : hitVFX;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);

        if (!isCrit)
            vfx.GetComponentInChildren<SpriteRenderer>().color = doDamageColor;
        else
        {
            if (entity.facingDirection == -1)
                vfx.transform.Rotate(0, 180, 0);
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
}
