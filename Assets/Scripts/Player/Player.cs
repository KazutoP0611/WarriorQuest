using System;
using System.Collections;
using UnityEngine;

public class Player : CharacterEntity
{
    private UI ui;

    public static event Action OnPlayerDead;
    public PlayerInputSet input { get; private set; }
    public Vector2 moveInput { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    public Player_VFX vfx { get; private set; }

    #region State Variables
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    #endregion

    [Header("Attack Details")]
    public Vector2[] attackVelocityArray;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 0.5f;
    private Coroutine queuedAttackCoroutine;

    [Header("Movement Details")]
    public float moveSpeed;
    public float jumpForce = 5;
    public Vector2 wallJumpForce;
    [Space]
    public float dashDuration = 0.25f;
    public float dashSpeed = 20;
    [Range(0, 1)] public float inAirMoveMultiplier;
    [Range(0, 1)] public float wallSlideMoveMultiplier;

    protected override void Awake()
    {
        base.Awake();

        ui = FindFirstObjectByType<UI>();
        input = new PlayerInputSet();
        skillManager = GetComponent<Player_SkillManager>();
        vfx = GetComponent<Player_VFX>();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += value => moveInput = value.ReadValue<Vector2>();
        input.Player.Movement.canceled += value => moveInput = Vector2.zero;

        input.Player.ToggleSkillTreeUI.performed += value => ui.ToggleSkillTreeUI();
        input.Player.Spell.performed += value => skillManager.shard.TryUseSkill();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCoroutine != null)
            StopCoroutine(queuedAttackCoroutine);

        queuedAttackCoroutine = StartCoroutine(EnterAttackStateWithDelayCoroutine());
    }

    private IEnumerator EnterAttackStateWithDelayCoroutine()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }

    public void TeleportPlayer(Vector3 position) => transform.position = position;

    public override void CharacterOnDead()
    {
        base.CharacterOnDead();

        if (slowDownCoroutine != null)
            StopCoroutine(slowDownCoroutine);

        OnPlayerDead?.Invoke();
        stateMachine.ChangeState(deadState);
    }

    protected override IEnumerator SlowDownCharacterCo(float duration, float slowMultiplier)
    {
        float defaultMoveSpeed = moveSpeed;
        float defaultJumpForce = jumpForce;
        float defaultAnimSpeed = anim.speed;
        Vector2 defaultWallJumpForce = wallJumpForce;
        Vector2 defaultJumpAttackVelocity = jumpAttackVelocity;
        Vector2[] defaultAttackVelocity = new Vector2[attackVelocityArray.Length];
        Array.Copy(attackVelocityArray, defaultAttackVelocity, attackVelocityArray.Length);

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed *= speedMultiplier;
        jumpForce *= speedMultiplier;
        anim.speed *= speedMultiplier;
        wallJumpForce *= speedMultiplier;
        jumpAttackVelocity *= speedMultiplier;

        for (int i = 0; i < attackVelocityArray.Length; i++)
        {
            attackVelocityArray[i] *= speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        anim.speed = defaultAnimSpeed;
        wallJumpForce = defaultWallJumpForce;
        jumpAttackVelocity = defaultJumpAttackVelocity;
        //attackVelocityArray = defaultAttackVelocity;

        for (int i = 0; i < attackVelocityArray.Length; i++)
        {
            attackVelocityArray[i] = defaultAttackVelocity[i];
        }
    }
}
