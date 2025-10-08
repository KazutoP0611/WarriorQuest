using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamagable
{
    [SerializeField] protected float maxHP = 100.0f;
    [SerializeField] protected float currentHP;
    [SerializeField] protected bool isDead = false;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] private float heavyKnockbackDuration = 0.5f;

    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDamageThreshold = 0.3f;

    private Slider healthBar;
    private Entity_VFX entityVFX;
    private CharacterEntity charEntity;

    protected virtual void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();
        entityVFX = GetComponent<Entity_VFX>();
        charEntity = GetComponent<CharacterEntity>();
        currentHP = maxHP;
        UpdateHealthBar();
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return;

        Vector2 knockback = CalculateKnockback(damage, damageDealer);
        float duration = CalculateDuration(damage);

        charEntity?.RecieveKnockback(knockback, duration);
        entityVFX?.PlayOnDamageVFX();
        ReduceHP(damage);
    }

    protected void ReduceHP(float damage)
    {
        currentHP -= damage;
        UpdateHealthBar();

        if (currentHP <= 0)
            Die();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;
        healthBar.value = currentHP / maxHP;
    }

    protected void Die()
    {
        isDead = true;
        charEntity.CharacterOnDead();
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;
        knockback.x *= direction;
        return knockback;
    }

    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;

    private bool IsHeavyDamage(float damage) => (damage / maxHP) >= heavyDamageThreshold;
}
