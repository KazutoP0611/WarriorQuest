using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Enemy : CharacterEntity
{
    public Enemy_IdleState enemyIdleState;
    public Enemy_MoveState enemyMoveState;
    public Enemy_AttackState enemyAttackState;
    public Enemy_BattleState enemyBattleState;

    [Header("Battle Details")]
    public float battleMoveSpeed = 3.0f;
    public float attackDistance = 2.0f;
    public float battleTimeDuration = 5.0f;
    public float minRetreatDistance = 1.0f;
    public Vector2 retreatVelocity;

    [Header("Movement Details")]
    //public float idleTime;
    public bool useRandomIdleTime = true;
    public Vector2 idleTimeRange;
    public float moveSpeed = 1.4f;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;

    [Header("Player Detection")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform playerDetectionTransform;
    [SerializeField] private float playerCheckDistance = 10;
    public Transform playerTransform { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        enemyIdleState = new Enemy_IdleState(this, stateMachine, "idle");
        enemyMoveState = new Enemy_MoveState(this, stateMachine, "move");
        enemyAttackState = new Enemy_AttackState(this, stateMachine, "attack");
        enemyBattleState = new Enemy_BattleState(this, stateMachine, "battle");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(enemyIdleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            playerDetectionTransform.position,
            Vector2.right * facingDirection,
            playerCheckDistance,
            playerLayer | groundLayer
        );

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    public void TryEnterBattleState(Transform playerTransform)
    {
        if (stateMachine.currentState == enemyBattleState || stateMachine.currentState == enemyAttackState)
            return;

        this.playerTransform = playerTransform;
        stateMachine.ChangeState(enemyBattleState);
    }

    public Transform GetPlayerReference()
    {
        if (playerTransform == null)
            playerTransform = PlayerDetected().transform;

        return playerTransform;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            playerDetectionTransform.position,
            new Vector2(playerDetectionTransform.position.x + (facingDirection * playerCheckDistance), playerDetectionTransform.position.y)
        );

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            playerDetectionTransform.position,
            new Vector2(playerDetectionTransform.position.x + (facingDirection * attackDistance), playerDetectionTransform.position.y)
        );

        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            playerDetectionTransform.position,
            new Vector2(playerDetectionTransform.position.x + (facingDirection * minRetreatDistance), playerDetectionTransform.position.y)
        );
    }
}
