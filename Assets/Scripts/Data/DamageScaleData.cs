using System;
using UnityEngine;

[Serializable]
public class DamageScaleData
{
    [Header("Damage Types")]
    public float physicalDamageScale = 1.0f;
    public float elementalDamageScale = 1.0f;

    [Header("Chill Effect")]
    public float chillDuration = 3.0f;
    public float chillSlowMultiplier = 0.2f;

    [Header("Burn Effect")]
    public float burnDuration = 3.0f;
    public float burnDamageScale = 1.0f;

    [Header("Shock Effect")]
    public float shockDuration = 3.0f;
    public float shockDamageScale = 1.0f;
    public float shockCharge = 0.4f;
}
