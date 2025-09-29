using UnityEngine;

public class Chest : MonoBehaviour, IDamagable
{
    private Rigidbody2D rb;
    private Animator anim;
    private Entity_VFX entityVFX;

    [Header("Open Details")]
    [SerializeField] private Vector2 knockback;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        entityVFX = GetComponent<Entity_VFX>();
    }

    public void TakeDamage(float damage, Transform damageDealer)
    {
        entityVFX.PlayOnDamageVFX();
        anim.SetBool("open", true);
        rb.linearVelocity = knockback;
        rb.angularVelocity = Random.Range(-60f, 60f);
    }
}
