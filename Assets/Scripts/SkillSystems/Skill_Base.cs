using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    private float lastTimeUsed;

    public DamageScaleData damageScaleData { get; private set; }

    [Header("General Details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType skillUpgradeType;
    [SerializeField] protected float cooldownTime;

    protected bool OnCoolDown() => Time.time < lastTimeUsed + cooldownTime;
    private void ResetCooldownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction; //make last time player used this skill more closer to current time
    private void ResetCooldown() => lastTimeUsed = Time.time;
    protected bool IsSkillUnlocked(SkillUpgradeType checkSkillUpgradeType) => checkSkillUpgradeType == skillUpgradeType;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public Player player { get; private set; }

    protected virtual void Awake()
    {
        player = GetComponentInParent<Player>();

        //Make lastTimeUsed less than Time.time when start playing
        //So player can use skill immediately when game start

        //No it's too slow, before it can set cooldownTime (6 or 3 or 8 seconds, lastTimeUsed is already 0), so when system set skillUpgradeType, and set cooldowntime
        //cooldownTime + lastTimeUsed is already > Time.time, so we have to wait.
        lastTimeUsed = lastTimeUsed - cooldownTime;
        //So I set cooldownTime in Skill Shard component to 10, to make lastTimeUsed = -10, so player can use shard spell immidietely after start game.
    }

    public virtual void TryUseSkill() { }

    public void SetSkillUpgrade(UpgradeData upgradeData)
    {
        skillUpgradeType = upgradeData.skillUpgradeType;
        cooldownTime = upgradeData.cooldownTime;
        damageScaleData = upgradeData.damageScaleData;
    }

    public bool CanUseSkill()
    {
        
        if (skillUpgradeType == SkillUpgradeType.None)
            return false;

        if (OnCoolDown())
        {
            Debug.LogWarning($"{name} is on cooldown.\n");
            return false;
        }

        return true;
    }
}
