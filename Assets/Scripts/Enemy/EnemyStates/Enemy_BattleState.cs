using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform playerTransform;
    private float lastTimeWasInBattle;

    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        UpdateBattleTimer();

        if (playerTransform == null)
            playerTransform = enemy.GetPlayerReference();

        if (ShouldRetreat())
        {
            rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected())
            UpdateBattleTimer();

        if (BattleTimeOver())
            stateMachine.ChangeState(enemy.enemyIdleState);

        if (WithinAttackRange() && enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.enemyAttackState);
        else
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.linearVelocity.y);
    }

    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;

    private bool WithinAttackRange() => DistanceToPlayer() <= enemy.attackDistance;

    private bool BattleTimeOver() => Time.time > (lastTimeWasInBattle + enemy.battleTimeDuration);

    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;

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
}
