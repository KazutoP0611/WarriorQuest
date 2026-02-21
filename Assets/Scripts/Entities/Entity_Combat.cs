using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public DamageScaleData basicAttackScale;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheckTransform;
    [SerializeField] private float targetCheckRadius = 1.0f;
    [SerializeField] private LayerMask targetLayer;

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

            AttackData attackData = stats.GetAttackData(basicAttackScale);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

            float physicalDamage = attackData.physicalDamage;
            float elementalDamage = attackData.elementalDamage;
            ElementType element = attackData.element;

            bool gotHit = damagable.TakeDamage(physicalDamage, elementalDamage, element, transform);

            if (element != ElementType.None)
                statusHandler.ApplyStatusEffect(element, attackData.elementalEffectData);

            if (gotHit)
                entityVFX.CreateOnHitVFX(target.transform, attackData.isCrit, element);
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
