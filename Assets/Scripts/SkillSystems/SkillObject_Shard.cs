using System;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    [SerializeField] private GameObject vfxPrefab;

    public void SetupShard(float detonationTime)
    {
        Invoke(nameof(Explode), detonationTime);
    }

    private void Explode()
    {
        DamageEnemyInRadius(transform, checkRadius);

        Instantiate(vfxPrefab, transform.position, Quaternion.identity);
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
