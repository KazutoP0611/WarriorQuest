using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    protected override void Die()
    {
        base.Die();

        player.Dead(); // disable input;
       
        //GameManager.instance.SetLastPlayerPosition(transform.position);
        //GameManager.instance.RestartScene();
    }
}
