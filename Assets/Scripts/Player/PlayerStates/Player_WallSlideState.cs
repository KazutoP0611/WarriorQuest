using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        HandleWallSlide();
        HandleJumpWhileWallSlide();

        if (!player.wallDetected)
            stateMachine.ChangeState(player.fallState);

        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);

            if (player.facingDirection != player.moveInput.x)
                player.Flip();
        }
    }

    private void HandleJumpWhileWallSlide()
    {
        if (input.Player.Jump.WasPerformedThisFrame())
        {
            if (player.moveInput.x != 0)
            {
                if (player.moveInput.x == player.facingDirection)
                    stateMachine.ChangeState(player.jumpState); //I don't know if this should be jump or wallJump, but I can adjust it later.
                else
                    stateMachine.ChangeState(player.wallJumpState);
            }
            else
            {
                stateMachine.ChangeState(player.fallState);
                player.Flip();
            }
        }
    }

    private void HandleWallSlide()
    {
        if (player.moveInput.y < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
        else
            player.SetVelocity(0, rb.linearVelocity.y * player.wallSlideMoveMultiplier);
    }
}
