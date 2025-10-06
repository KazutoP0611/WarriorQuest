using UnityEngine;

public class Enemy_VFX : Entity_VFX
{
    [Header("Counter Attack VFX")]
    [SerializeField] private GameObject attackAlert;

    protected override void Awake()
    {
        base.Awake();

        EnableAttackAlert(false);
    }

    public void EnableAttackAlert(bool enable) => attackAlert.SetActive(enable);
}
