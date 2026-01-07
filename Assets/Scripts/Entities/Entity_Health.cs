using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamagable
{
    [SerializeField] protected float currentHP;
    [SerializeField] protected bool isDead = false;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] private float heavyKnockbackDuration = 0.5f;

    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDamageThreshold = 0.3f;

    private CharacterEntity charEntity;
    private Entity_Stats stats;
    private Entity_VFX entityVFX;
    private Slider healthBar;

    protected virtual void Awake()
    {
        charEntity = GetComponent<CharacterEntity>();
        stats = GetComponent<Entity_Stats>();
        entityVFX = GetComponent<Entity_VFX>();

        healthBar = GetComponentInChildren<Slider>();

        currentHP = stats.GetMaxHealth();
        UpdateHealthBar();
    }

    public virtual bool TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return false;

        if (AttackEvaded())
            return false;

        Entity_Stats attackStats = damageDealer.GetComponent<Entity_Stats>();
        float attackerArmorReduction = attackStats != null ? attackStats.GetArmorReduction() : 0;

        float mitigation = stats.GetArmorMitigation(attackerArmorReduction);
        float finalDamage = damage * (1 - mitigation);

        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);

        charEntity?.RecieveKnockback(knockback, duration);
        entityVFX?.PlayOnDamageVFX();
        ReduceHP(finalDamage);
        
        Debug.Log($"{finalDamage}");

        return true;
    }

    private bool AttackEvaded() => Random.Range(0, 100) < stats.GetEvasion();

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
        healthBar.value = currentHP / stats.GetMaxHealth();
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

    private bool IsHeavyDamage(float damage) => (damage / stats.GetMaxHealth()) >= heavyDamageThreshold;
}
