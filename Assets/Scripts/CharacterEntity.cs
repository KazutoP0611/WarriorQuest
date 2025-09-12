using UnityEngine;

public class CharacterEntity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    protected StateMachine stateMachine;

    private bool facingRight = true;
    public int facingDirection { get; private set; } = 1;


    [Header("Collision Detection")]
    [SerializeField] private float groungCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform primaryWallDetector;
    [SerializeField] private Transform secondaryWallDetector;
    [SerializeField] private Transform groundCheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }
    

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {

    }

    private void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    private void HandleFlip(float xVelocity)
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groungCheckDistance, 0));
        Gizmos.DrawLine(primaryWallDetector.position, primaryWallDetector.position + new Vector3(wallCheckDistance * facingDirection, 0, 0));
        
        if (secondaryWallDetector)
            Gizmos.DrawLine(secondaryWallDetector.position, secondaryWallDetector.position + new Vector3(wallCheckDistance * facingDirection, 0, 0));
    }
}
