using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterEntity : MonoBehaviour
{
    public event Action OnFlipped;

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Entity_Stats stats { get; private set; }
    public int facingDirection { get; private set; } = 1;

    protected StateMachine stateMachine;
    protected Coroutine slowDownCoroutine;

    private bool facingRight = true;

    [Header("Collision Detection")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] private float groungCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform primaryWallDetector;
    [SerializeField] private Transform secondaryWallDetector;
    [SerializeField] private Transform groundCheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    //Condition Variables
    private bool isKnocked;
    private Coroutine knockbackCoroutine;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponentInChildren<Entity_Stats>();

        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    public void RecieveKnockback(Vector2 knockback, float duration)
    {
        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        knockbackCoroutine = StartCoroutine(Knockback(knockback, duration));
    }

    private IEnumerator Knockback(Vector2 knockback, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = knockback;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        isKnocked = false;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
            return;

        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && facingRight == false)
            Flip();
        else if (xVelocity < 0 && facingRight)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDirection = facingRight ? 1 : -1;

        OnFlipped?.Invoke();
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groungCheckDistance, groundLayer);

        if (secondaryWallDetector)
        {
            wallDetected = Physics2D.Raycast(primaryWallDetector.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer)
                && Physics2D.Raycast(secondaryWallDetector.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer);
        }
        else
            wallDetected = Physics2D.Raycast(primaryWallDetector.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer);
    }

    public virtual void CharacterOnDead()
    {

    }

    public virtual void SlowDownCharacterBy(float duration, float slowMultiplier)
    {
        if (slowDownCoroutine != null)
            StopCoroutine(slowDownCoroutine);

        slowDownCoroutine = StartCoroutine(SlowDownCharacterCo(duration, slowMultiplier));
    }

    protected virtual IEnumerator SlowDownCharacterCo(float duration, float slowMultiplier)
    {
        //Debug.Log("Slow Down");
        yield return null;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groungCheckDistance, 0));
        Gizmos.DrawLine(primaryWallDetector.position, primaryWallDetector.position + new Vector3(wallCheckDistance * facingDirection, 0, 0));
        
        if (secondaryWallDetector)
            Gizmos.DrawLine(secondaryWallDetector.position, secondaryWallDetector.position + new Vector3(wallCheckDistance * facingDirection, 0, 0));
    }
}
