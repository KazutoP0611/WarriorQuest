using System;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    public event Action OnExplode;

    private Skill_Shard skillShard;

    [SerializeField] private GameObject vfxPrefab;

    private Transform target;
    private float speed;

    private void Update()
    {
        if (target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void MoveTowardClosestTarget(float speed)
    {
        this.speed = speed;
        target = FindClosestEnemy();
    }

    public void SetupShard(Skill_Shard skillShard)
    {
        this.skillShard = skillShard;

        playerStats = skillShard.player.stats;
        damageScaleData = skillShard.damageScaleData;

        float detonationTime = skillShard.GetDetonationTime();

        Invoke(nameof(Explode), detonationTime);
    }

    public void Explode()
    {
        DamageEnemyInRadius(transform, checkDamageRadius);
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);

        OnExplode?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;

        Debug.LogWarning("Found");
        Explode();
    }
}
