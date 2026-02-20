using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    protected Entity_Stats playerStats;
    protected DamageScaleData damageScaleData;

    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform targetCheckTransform;
    [SerializeField] protected float checkDamageRadius = 1.0f;
    [SerializeField] protected float checkClosestEnemyRadius = 10.0f;

    protected virtual void DamageEnemyInRadius(Transform t, float radius)
    {
        foreach (var collider in GetEnemiesAround(t, radius))
        {
            IDamagable damagable = collider.GetComponent<IDamagable>();
            if (damagable == null)
                continue;

            ElementalEffectData elementalEffectData = new ElementalEffectData(playerStats, damageScaleData);

            float physicalDamage = playerStats.GetPhysicalDamage(out bool isCrit, damageScaleData.physicalDamageScale);
            float elementalDamage = playerStats.GetElemetalDamage(out ElementType element, damageScaleData.elementalDamageScale);

            damagable.TakeDamage(physicalDamage, elementalDamage, element, transform);

            if (element != ElementType.None)
                collider.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(element, elementalEffectData);
        }
    }

    protected Transform FindClosestEnemy()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in GetEnemiesAround(transform, checkClosestEnemyRadius))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy.transform;
            }
        }
        return target;
    }

    protected Collider2D[] GetEnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, enemyLayer);
    }

    protected virtual void OnDrawGizmos()
    {
        if (targetCheckTransform == null)
            targetCheckTransform = transform;

        Gizmos.DrawWireSphere(targetCheckTransform.position, checkDamageRadius);
    }
}
