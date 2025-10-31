using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();

    public override bool TakeDamage(float damage, Transform damageDealer)
    {
        bool gotHit = base.TakeDamage(damage, damageDealer);

        if (!gotHit)
            return false;

        if (damageDealer.GetComponent<Player>() != null)
            enemy.TryEnterBattleState(damageDealer);

        return true;
    }
}
