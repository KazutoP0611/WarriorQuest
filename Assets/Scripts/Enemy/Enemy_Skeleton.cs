using UnityEngine;

public class Enemy_Skeleton : Enemy, ICounterable
{
    public bool CanBeCounterd { get { return canBeStunned; } } // { get => canBeStunned; set => canBeStunned = value; }

    //protected override void Awake()
    //{
    //    base.Awake();

    //    enemyIdleState = new Enemy_IdleState(this, stateMachine, "idle");
    //    enemyMoveState = new Enemy_MoveState(this, stateMachine, "move");
    //    enemyAttackState = new Enemy_AttackState(this, stateMachine, "attack");
    //    enemyBattleState = new Enemy_BattleState(this, stateMachine, "battle");
    //    enemyStunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
    //    enemyDeadState = new Enemy_DeadState(this, stateMachine, "idle");
    //}

    //protected override void Start()
    //{
    //    base.Start();

    //    stateMachine.Initialize(enemyIdleState);
    //}

    //protected override void Update()
    //{
    //    base.Update();

    //    if (Input.GetKey(KeyCode.F))
    //        HandleCounter();
    //}

    [ContextMenu("Stun Enemy")]
    public void HandleCounter()
    {
        if (CanBeCounterd)
            stateMachine.ChangeState(enemyStunnedState);
    }
}
