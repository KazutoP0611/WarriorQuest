using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;
    private Entity_Health playerHealth;

    private Coroutine normalRechargeCoroutine;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonationTime = 2.0f;

    [Header("Moving Shard Upgrade")]
    [SerializeField] private float shardSpeed = 7.0f;

    [Header("Multicast Shard Upgrade")]
    [SerializeField] private int maxCastAmount = 3;
    [SerializeField] private int currentUsableAmount;
    [SerializeField] private bool isRecharging;
    [Space]
    [SerializeField] private float fullRechargeTime = 6.0f;

    [Header("Teleport Shard Upgrade")]
    [SerializeField] private float shardExistDuration = 5.0f;

    [Header("Teleport and Rewind Hp Upgrade")]
    [SerializeField] private float savedHealth;

    protected override void Awake()
    {
        base.Awake();

        playerHealth = GetComponentInParent<Entity_Health>();
        currentUsableAmount = maxCastAmount;
    }

    public override void TryUseSkill()
    {
        base.TryUseSkill();

        if (CanUseSkill() == false)
            return;

        if (IsSkillUnlocked(SkillUpgradeType.Shard))
            HandleShardRegular();

        if (IsSkillUnlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();

        if (IsSkillUnlocked(SkillUpgradeType.Shard_MultiCast))
            HandleMultiCastShard();

        if (IsSkillUnlocked(SkillUpgradeType.Shard_Teleport))
            HandleShardTeleport();

        if (IsSkillUnlocked(SkillUpgradeType.Shard_TeleportRewindHp))
            HandleShardTeleportAndRewindHp();
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardClosestTarget(shardSpeed);

        SetSkillOnCooldown();
    }

    private void HandleMultiCastShard()
    {
        if (currentUsableAmount <= 0)
            return;

        CreateShard();
        currentUsableAmount--;
        currentShard.MoveTowardClosestTarget(shardSpeed);


        if (currentUsableAmount <= 0)
        {
            if (normalRechargeCoroutine != null)
                StopCoroutine(normalRechargeCoroutine);

            //Panelty for used up all of shard, player will have to wait longer for full recharge;
            StartCoroutine(RechargeFullShardCo());
        }
        else
        {
            if (isRecharging == false)
                normalRechargeCoroutine = StartCoroutine(RechargeShardCo());
        }
    }

    private void HandleShardTeleport()
    {
        if (currentShard == null)
        {
            CreateShard();
            currentShard.OnExplode += ForceCooldown;
        }
        else
        {
            SwapPlayerAndShardPosition();
            currentShard.Explode();
        }
    }

    private void HandleShardTeleportAndRewindHp()
    {
        if (currentShard == null)
        {
            CreateShard();
            currentShard.OnExplode += ForceCooldown;

            //Save player health at the moment shard is created.
            savedHealth = playerHealth.GetHealthIn01();
        }
        else
        {
            SwapPlayerAndShardPosition();

            //After swap player's and shard's places, restore player's health to saved health.
            playerHealth.SetHealthIn01(savedHealth);

            currentShard.Explode();
        }
    }

    private IEnumerator RechargeShardCo()
    {
        // Set this for player can't recharge shard too fast.
        isRecharging = true;

        while (currentUsableAmount < maxCastAmount)
        {
            yield return new WaitForSeconds(cooldownTime); //In this case, we use cooldownTime as recharge intervel time.
            currentUsableAmount++;  
        }

        //Now player can recharge again.
        isRecharging = false;
    }

    private IEnumerator RechargeFullShardCo()
    {
        isRecharging = true;

        yield return new WaitForSeconds(fullRechargeTime);
        currentUsableAmount = maxCastAmount;

        isRecharging = false;
    }

    private void SwapPlayerAndShardPosition()
    {
        Vector2 shardPosition = currentShard.transform.position;
        Vector2 playerPosition = player.transform.position;

        player.TeleportPlayer(shardPosition);
        currentShard.transform.position = playerPosition;
    }

    private void CreateShard()
    {
        GameObject shardObject = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shardObject.GetComponent<SkillObject_Shard>();

        float detonationTime = GetDetonationTime();
        currentShard.SetupShard(detonationTime);
    }

    private float GetDetonationTime()
    {
        if (IsSkillUnlocked(SkillUpgradeType.Shard_Teleport) || IsSkillUnlocked(SkillUpgradeType.Shard_TeleportRewindHp))
            return shardExistDuration;

        return detonationTime;
    }

    private void ForceCooldown()
    {
        if (OnCoolDown() == false)
        {
            SetSkillOnCooldown();
            currentShard.OnExplode -= ForceCooldown;
        }
    }
}
