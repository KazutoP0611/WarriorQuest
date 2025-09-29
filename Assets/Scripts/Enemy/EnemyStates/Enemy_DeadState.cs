using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    private Collider2D collider;

    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        collider = enemy.GetComponent<Collider2D>();
    }

    public override void Enter()
    {
        anim.enabled = false;
        collider.enabled = false;

        rb.gravityScale = enemy._fallGravityScale;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, enemy._onDeadJumpVelocity);

        stateMachine.SwitchOffStateMachine();
    }
}
