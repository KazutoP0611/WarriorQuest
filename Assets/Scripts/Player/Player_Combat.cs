using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter Attack Details")]
    [SerializeField] private float counterRecovery = 0.1f;

    public bool CounterAttackPerformed()
    {
        bool hasCounterSomebody = false;

        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();

            if (counterable != null)
            {
                counterable.HandleCounter();
                //need to refactor this logic a little, player should not go back to idle state immidiately after fail counter, one should stay still (counter animation);
                hasCounterSomebody = true;
            }
        }

        return hasCounterSomebody;
    }

    public float GetCounterRecoveryDuration() => counterRecovery;
}
