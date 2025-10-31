using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public float damage = 10.0f;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheckTransform;
    [SerializeField] private float targetCheckRadius = 1.0f;
    [SerializeField] private LayerMask targetLayer;

    private Entity_VFX vfx;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
    }

    public void PerformAttack()
    {
        foreach (Collider2D target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;

            bool gotHit = damagable.TakeDamage(damage, transform);

            if (gotHit)
                vfx.OnHitVFX(target.transform);
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
