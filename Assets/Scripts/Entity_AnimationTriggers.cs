using UnityEngine;
using UnityEngine.LowLevel;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private CharacterEntity character;

    private void Awake()
    {
        character = GetComponentInParent<CharacterEntity>();
    }

    private void CurrentStateTrigger()
    {
        character.CallAnimationTrigger();
    }
}
