using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    [Header("Target Detection")]
    [SerializeField] private Transform targetCheckTransform;
    [SerializeField] private float targetCheckRadius = 1.0f;
    [SerializeField] private LayerMask targetLayer;

    private Entity_VFX vfx;
    private Entity_Stats stats;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
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
            float elementalDamage = stats.GetElemetalDamage();
            bool gotHit = damagable.TakeDamage(damage, elementalDamage, transform);

            if (gotHit)
                vfx.OnHitVFX(target.transform, isCrit);
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
