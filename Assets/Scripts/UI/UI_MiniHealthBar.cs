using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private CharacterEntity entity;

    private void OnEnable()
    {
        entity = GetComponentInParent<CharacterEntity>();
        entity.OnFlipped += HandleFlip;
    }

    private void OnDisable() => entity.OnFlipped -= HandleFlip;

    private void HandleFlip()
    {
        transform.rotation = Quaternion.identity;
    }
}
