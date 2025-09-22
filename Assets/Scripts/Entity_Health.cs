using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    [SerializeField] protected float maxHP = 100.0f;
    [SerializeField] protected bool isDead = false;

    private float currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return;

        ReduceHP(damage);
    }

    protected void ReduceHP(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
            Die();
    }

    protected void Die()
    {
        isDead = true;
        Debug.LogWarning($"{gameObject.name} is Dead!!");
    }
}
