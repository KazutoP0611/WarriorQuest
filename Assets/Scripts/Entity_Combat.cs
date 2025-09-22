using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public float damage = 10.0f;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheckTransform;
    [SerializeField] private float targetCheckRadius = 1.0f;
    [SerializeField] private LayerMask targetLayer;

    public void PerformAttack()
    {
        foreach (Collider2D enemy in GetDetectedColliders())
        {
            Entity_Health targetHealth = enemy.GetComponent<Entity_Health>();
            targetHealth?.TakeDamage(damage, transform);
        }
    }

    private Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheckTransform.position, targetCheckRadius, targetLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheckTransform.position, targetCheckRadius);
    }
}
