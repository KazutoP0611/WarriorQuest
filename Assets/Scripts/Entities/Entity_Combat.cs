using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    [Header("Target Detection")]
    [SerializeField] private Transform targetCheckTransform;
    [SerializeField] private float targetCheckRadius = 1.0f;
    [SerializeField] private LayerMask targetLayer;

    [Header("Status Effect Details")]
    [SerializeField] private float defaultEffectDuration = 3.0f;
    [SerializeField] private float slowMultiplier = 0.2f;

    private Entity_VFX entityVFX;
    private Entity_Stats stats;

    private void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach (Collider2D target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;

            float damage = stats.GetPhysicalDamage(out bool isCrit);
            float elementalDamage = stats.GetElemetalDamage(out ElementType element);
            bool gotHit = damagable.TakeDamage(damage, elementalDamage, element, transform);

            if (gotHit)
            {
                ApplyStatusEffect(target.transform, element);

                entityVFX.UpdateOnHitVFXColor(element);
                entityVFX.OnHitVFX(target.transform, isCrit);
            }
        }
    }

    public void ApplyStatusEffect(Transform target, ElementType element)
    {
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
        if (statusHandler == null)
            return;

        if (statusHandler.CanBeApplied(element))
        {
            Debug.LogWarning($"Character {target.gameObject.name} get debuff element : {element}");
            switch (element)
            {
                case ElementType.Ice:
                    statusHandler.AppliedChillEffect(defaultEffectDuration, slowMultiplier);
                    break;
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
