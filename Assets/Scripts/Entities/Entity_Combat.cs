using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public DamageScaleData basicAttackScale;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheckTransform;
    [SerializeField] private float targetCheckRadius = 1.0f;
    [SerializeField] private LayerMask targetLayer;

    [Header("Status Effect Details")]
    [SerializeField] private float defaultEffectDuration = 3.0f;
    [SerializeField] private float slowMultiplier = 0.2f;
    [SerializeField] private float electrifiedChargeBuildUp = 0.4f;
    [Space]
    [SerializeField] private float fireScale = 0.8f;
    [SerializeField] private float lightningScale = 2.5f;

    private Entity_VFX entityVFX;
    private Entity_Stats stats;

    private void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        //float damage = stats.GetPhysicalDamage(out bool isCrit);
        //Debug.Log($"Current damage is {damage}");

        foreach (Collider2D target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;

            ElementalEffectData elemetalEffectData = new ElementalEffectData(stats, basicAttackScale);

            float damage = stats.GetPhysicalDamage(out bool isCrit);
            float elementalDamage = stats.GetElemetalDamage(out ElementType element);
            bool gotHit = damagable.TakeDamage(damage, elementalDamage, element, transform);

            if (element != ElementType.None)
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(element, elemetalEffectData);

            if (gotHit)
            {
                entityVFX.UpdateOnHitVFXColor(element);
                entityVFX.OnHitVFX(target.transform, isCrit);
            }
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheckTransform.position, targetCheckRadius, targetLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheckTransform.position, targetCheckRadius);
    }
}
