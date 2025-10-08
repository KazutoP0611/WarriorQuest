using UnityEngine;

public interface ICounterable
{
    public bool CanBeCounterd { get; }

    public void HandleCounter();
}
