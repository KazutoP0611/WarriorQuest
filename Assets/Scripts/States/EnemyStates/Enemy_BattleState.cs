using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform playerTransform;

    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //if (playerTransform == null)
        //    playerTransform = enemy.PlayerDetection().transform;

        //if (ShouldRetreat())
        //{
        //    rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
        //    enemy.HandleFlip(DirectionToPlayer());
        //}

        playerTransform = enemy.PlayerDetection().transform;
    }

    public override void Update()
    {
        base.Update();

        //if (BattleTimeOver())
        //    stateMachine.ChangeState(enemy.enemyIdleState);

        if (WithinAttackRange())
            stateMachine.ChangeState(enemy.enemyAttackState);
        else
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.linearVelocity.y);
    }

    private bool ShouldRetreat()
    {
        return true;
    }

    private bool WithinAttackRange()
    {
        return DistanceToPlayer() <= enemy.attackDistance;
    }

    private float DistanceToPlayer()
    {
        if (playerTransform == null)
            return float.MaxValue;

        return Mathf.Abs(playerTransform.position.x - enemy.transform.position.x);
    }

    private int DirectionToPlayer()
    {
        if (playerTransform == null)
            return 0;

        return playerTransform.position.x > enemy.transform.position.x ? 1 : -1;
    }

    private bool BattleTimeOver()
    {
        return true;
    }
}
