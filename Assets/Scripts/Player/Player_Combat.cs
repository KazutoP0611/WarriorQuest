using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter Attack Details")]
    [SerializeField] private float counterRecovery = 0.1f;

    public bool CounterAttackPerformed()
    {
        bool performedCounter = false;

        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();

            if (counterable == null)
                continue;

            if (counterable.CanBeCounterd)//use ".CanBeCountered" instead of checking null will make sure that That enemy is in the state that we, player, can counter them.
            {
                counterable.HandleCounter();
                performedCounter = true;
            }
        }

        return performedCounter;
    }

    public float GetCounterRecoveryDuration() => counterRecovery;
}
