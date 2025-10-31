using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity_VFX : MonoBehaviour
{
    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = 0.1f;

    [Header("On Doing Damage VFX")]
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private Color doDamageColor = Color.white;

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine onDamageCoroutine;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    public void OnHitVFX(Transform target)
    {
        GameObject vfx = Instantiate(hitVFX, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = doDamageColor;
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
