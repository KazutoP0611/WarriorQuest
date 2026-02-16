using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamagable
{
    private CharacterEntity charEntity;
    private Entity_Stats stats;
    private Entity_VFX entityVFX;
    private Slider healthBar;

    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isDead = false;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] private float heavyKnockbackDuration = 0.5f;

    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDamageThreshold = 0.3f;

    [Header("Health Regen")]
    [SerializeField] private float regenInterval = 1.0f;
    [SerializeField] private bool canRegen = true;

    protected virtual void Awake()
    {
        charEntity = GetComponent<CharacterEntity>();
        stats = GetComponent<Entity_Stats>();
        entityVFX = GetComponent<Entity_VFX>();

        healthBar = GetComponentInChildren<Slider>();

        currentHealth = stats.GetMaxHealth();
        UpdateHealthBar();

        InvokeRepeating(nameof(RegenerateHealth), 0.0f, regenInterval);
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead)
            return false;

        if (AttackEvaded())
            return false;

        Entity_Stats attackStats = damageDealer.GetComponent<Entity_Stats>();
        float attackerArmorReduction = attackStats != null ? attackStats.GetArmorReduction() : 0;

        float mitigation = stats.GetArmorMitigation(attackerArmorReduction);
        float physicalDamageTaken = damage * (1 - mitigation);

        float elementalResistance = stats.GetElementalResistance(element);
        float elementalDamageTaken = elementalDamage * (1 - elementalResistance);

        TakeKnockBack(damageDealer, physicalDamageTaken);
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);

        //Debug.Log($"First elemental damage : {elementalDamage} and Elemental damage taken : {elementalDamageTaken} and physical damage is {finalDamage}");

        return true;
    }

    private void TakeKnockBack(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);

        charEntity?.RecieveKnockback(knockback, duration);
    }

    private bool AttackEvaded() => Random.Range(0, 100) < stats.GetEvasion();

    private void RegenerateHealth()
    {
        if (!canRegen)
            return;

        float regenAmount = stats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }

    public void ReduceHealth(float damage)
    {
        entityVFX?.PlayOnDamageVFX();

        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
            Die();
    }

    public void IncreaseHealth(float healAmount)
    {
        if (isDead)
            return;

        float newHealth = currentHealth + healAmount;
        currentHealth = Mathf.Clamp(newHealth, 0, stats.GetMaxHealth());
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;
        healthBar.value = currentHealth / stats.GetMaxHealth();
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
