using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer;

    private const int ComboFirstIndex = 0;
    private int comboLimit = 2;
    private int attackIndex = 0;

    private int attackDirection;

    private float lastAttackTime = 0;
    private bool attackQueued = false;

    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (comboLimit != player.attackVelocityArray.Length - 1)
            comboLimit = player.attackVelocityArray.Length - 1;
    }

    public override void Enter()
    {
        base.Enter();

        SyncAttackSpeed();

        attackQueued = false;
        ResetComboIndexIfNeeded();

        attackDirection = player.moveInput.x != 0 ?
            ((int)player.moveInput.x) :
            player.facingDirection;

        anim.SetInteger("attackIndex", attackIndex);
        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();

        HandleAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
            QueuedNextAttack();

        if (triggerCalled)
            HandleComboAttack();
    }

    private void HandleComboAttack()
    {
        if (attackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        lastAttackTime = Time.time;
        attackIndex++;
    }

    private void QueuedNextAttack()
    {
        if (attackIndex < comboLimit)
            attackQueued = true;
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocityArray[attackIndex];
        attackVelocityTimer = player.attackVelocityDuration;

        player.SetVelocity(
            attackVelocity.x * attackDirection,
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
