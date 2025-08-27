using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public PlayerInputSet input { get; private set; }


    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }


    [Header("Movement Details")]
    public Vector2 moveInput { get; private set; }
    public float moveSpeed;
    public float jumpForce = 5;
    public Vector2 wallJumpForce;
    [Space]
    public float dashDuration = 0.25f;
    public float dashSpeed = 20;

    [Range(0,1)] public float inAirMoveMultiplier;
    [Range(0, 1)] public float wallSlideMoveMultiplier;
    private bool facingRight = true;
    public int facingDirection { get; private set; } = 1;


    [Header("Collision Detection")]
    [SerializeField] private float groungCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask groundLayer;

    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }


    private StateMachine stateMachine;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
        input = new PlayerInputSet();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += value => { moveInput = value.ReadValue<Vector2>(); };
        input.Player.Movement.canceled += value => { moveInput = Vector2.zero; };
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);    }

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
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groungCheckDistance, groundLayer);

        wallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groungCheckDistance, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance * facingDirection, 0, 0));
    }
}
