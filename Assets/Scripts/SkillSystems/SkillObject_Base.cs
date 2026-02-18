using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform targetCheckTransform;
    [SerializeField] protected float checkRadius = 1.0f;

    protected virtual void DamageEnemyInRadius(Transform t, float radius)
    {
        foreach (var collider in GetEnemiesAround(t, radius))
        {
            IDamagable damagable = collider.GetComponent<IDamagable>();
            if (damagable == null)
                continue;

            damagable.TakeDamage(1, 1, ElementType.None, transform);
        }
    }

    protected Collider2D[] GetEnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius);
    }

    protected virtual void OnDrawGizmos()
    {
        if (targetCheckTransform == null)
            targetCheckTransform = transform;

        Gizmos.DrawWireSphere(targetCheckTransform.position, checkRadius);
    }
}
