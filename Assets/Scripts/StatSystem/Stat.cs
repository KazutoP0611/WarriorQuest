using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    private bool wasModified = true;
    private float modifiedValue;

    public float GetValue()
    {
        if (wasModified)
        {
            modifiedValue = GetModifiedValue();
            wasModified = false;
        }

        return modifiedValue;
    }

    public void AddModifier(float value, string source)
    {
        StatModifier newStatModifier = new StatModifier(value, source);
        modifiers.Add(newStatModifier);

        wasModified = true;
    }

    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(modifier => modifier.source == source);
        wasModified = true;
    }

    public float GetModifiedValue()
    {
        float modValue = baseValue;

        foreach (var modifier in modifiers)
        {
            modValue += modifier.value;
        }

        return modValue;
    }

    public void SetBaseValue(float value) => baseValue = value;
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
