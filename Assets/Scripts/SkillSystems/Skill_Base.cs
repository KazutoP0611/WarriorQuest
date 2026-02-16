using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    private float lastTimeUsed;

    [Header("General Details")]
    [SerializeField] private float cooldownTime;

    private bool OnCoolDown() => Time.time < lastTimeUsed + cooldownTime;
    private void ResetCooldownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction; //make last time player used this skill more closer to current time
    private void ResetCooldown() => lastTimeUsed = Time.time;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;

    protected virtual void Awake()
    {
        //Make lastTimeUsed less than Time.time when start playing
        //So player can use skill immediately when game start
        lastTimeUsed = lastTimeUsed - cooldownTime;
    }

    public bool CanUseSkill()
    {
        if (OnCoolDown())
        {
            Debug.LogWarning($"{name} is one cooldown.");
            return false;
        }

        return true;
    }
}
