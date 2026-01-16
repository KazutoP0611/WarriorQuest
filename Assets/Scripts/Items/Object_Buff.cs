using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Buff
{
    public StatType type;
    public float value;
}

public class Object_Buff : MonoBehaviour
{
    [Header("Buff Details")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 5.0f;
    [SerializeField] private bool canBeUsed = true;

    [Header("Float Movement Details")]
    [SerializeField] private float floatSpeed = 1.0f;
    [SerializeField] private float floatRange = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Entity_Stats statsToModify;
    private Vector3 startPosition;

    private Coroutine buffCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        startPosition = transform.position;
    }

    private void Update()
    {
        float yOffSet = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffSet);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeUsed)
            return;

        statsToModify = collision.GetComponent<Entity_Stats>();

        if (buffCoroutine != null)
            StopCoroutine(buffCoroutine);

        buffCoroutine = StartCoroutine(BuffCo(buffDuration));
    }

    private IEnumerator BuffCo(float duration)
    {
        canBeUsed = false;
        spriteRenderer.color = Color.clear;

        ApplyOrRemoveBuff(true);
        yield return new WaitForSeconds(duration);

        ApplyOrRemoveBuff(false);
        //yield return new WaitForSeconds(0.1f);
        Destroy(gameObject, 0.2f);
    }

    private void ApplyOrRemoveBuff(bool apply)
    {
        foreach (var buff in buffs)
        {
            if (apply)
                statsToModify.GetStatByType(buff.type).AddModifier(buff.value, buffName);
            else
                statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
        }
    }
}
