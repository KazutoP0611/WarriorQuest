using UnityEngine;

public class Player_Health : Entity_Health
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            Die();
    }

    protected override void Die()
    {
        base.Die();

        GameManager.instance.SetLastPlayerPosition(transform.position);
        GameManager.instance.RestartScene();
    }
}
