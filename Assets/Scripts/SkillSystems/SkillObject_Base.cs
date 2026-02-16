using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform targetCheckTransform;
    [SerializeField] private float checkRadius = 1.0f;

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
