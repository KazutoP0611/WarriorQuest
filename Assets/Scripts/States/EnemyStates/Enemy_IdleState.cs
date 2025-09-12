using UnityEngine;

public class Enemy_IdleState : EnemyState
{
    public Enemy_IdleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.useRandomIdleTime ? Random.Range((int)enemy.idleTimeRange.x, (int)enemy.idleTimeRange.y) : enemy.idleTimeRange.x;
        Debug.LogWarning($"Idle time of this wait time: {stateTimer}");
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.enemyMoveState);
    }
}
