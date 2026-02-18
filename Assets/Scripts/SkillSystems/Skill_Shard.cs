using UnityEngine;

public class Skill_Shard : Skill_Base
{
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonationTime = 2.0f;

    public void CreateShard()
    {
        GameObject shardObject = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shardObject.GetComponent<SkillObject_Shard>().SetupShard(detonationTime);
    }
}
