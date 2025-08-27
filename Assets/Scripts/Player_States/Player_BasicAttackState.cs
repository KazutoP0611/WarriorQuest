using UnityEngine;

public class Player_BasicAttackState : Player_GroundedState
{
    private float attackVelocityTimer;

    private const int ComboFirstIndex = 0;
    private int comboLimit = 2;
    private int attackIndex = 0;

    private float lastAttackTime = 0;

    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ResetComboIndexIfNeeded();

        anim.SetInteger("attackIndex", attackIndex);
        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();

        HandleAttackVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        lastAttackTime = Time.time;
        attackIndex++;
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocityArray[attackIndex];
        attackVelocityTimer = player.attackVelocityDuration;

        player.SetVelocity(
            attackVelocity.x * player.facingDirection,
            attackVelocity.y
        );
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
    }

    private void ResetComboIndexIfNeeded()
    {
        if (Time.time - lastAttackTime > player.comboResetTime)
            attackIndex = ComboFirstIndex;

        if (attackIndex > comboLimit)
            attackIndex = ComboFirstIndex;
    }
}
