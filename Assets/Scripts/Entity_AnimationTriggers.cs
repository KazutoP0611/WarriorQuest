using UnityEngine;
using UnityEngine.LowLevel;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private CharacterEntity character;
    private Entity_Combat entityCombat;

    private void Awake()
    {
        character = GetComponentInParent<CharacterEntity>();
        entityCombat = GetComponentInParent<Entity_Combat>();
    }

    private void CurrentStateTrigger()
    {
        character.CurrentStateAnimationTrigger();
    }

    private void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }
}
