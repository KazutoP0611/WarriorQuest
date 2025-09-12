using UnityEngine;

public class Enemy : CharacterEntity
{
    public Enemy_IdleState enemyIdleState;
    public Enemy_MoveState enemyMoveState;
    public Enemy_AttackState enemyAttackState;

    [Header("Movement Details")]
    //public float idleTime;
    public bool useRandomIdleTime = true;
    public Vector2 idleTimeRange;
    public float moveSpeed = 1.4f;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;

    protected override void Awake()
    {
        base.Awake();

        enemyIdleState = new Enemy_IdleState(this, stateMachine, "idle");
        enemyMoveState = new Enemy_MoveState(this, stateMachine, "move");
        enemyAttackState = new Enemy_AttackState(this, stateMachine, "attack");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(enemyIdleState);
    }
}
