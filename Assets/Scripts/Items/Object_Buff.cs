using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Object_Buff : MonoBehaviour
{
    [Header("Buff Details")]
    [SerializeField] private float buffDuration = 5.0f;
    [SerializeField] private bool canBeUsed = true;

    [Header("Float Movement Details")]
    [SerializeField] private float floatSpeed = 1.0f;
    [SerializeField] private float floatRange = 0.1f;

    private SpriteRenderer spriteRenderer;
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

        if (buffCoroutine != null)
            StopCoroutine(buffCoroutine);

        buffCoroutine = StartCoroutine(BuffCo(buffDuration));
    }

    private IEnumerator BuffCo(float duration)
    {
        canBeUsed = false;
        spriteRenderer.color = Color.clear;
        Debug.LogWarning("Buff is applied!!");

        yield return new WaitForSeconds(duration);
        Debug.LogWarning("Buff is removed!!");
        Destroy(gameObject, 0.2f);
    }
}
