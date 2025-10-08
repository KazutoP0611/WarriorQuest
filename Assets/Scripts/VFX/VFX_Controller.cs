using System.Collections;
using UnityEngine;

public class VFX_Controller : MonoBehaviour
{
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyInSecs = 1.0f;

    [Header("Random After Be Spawned")]
    [SerializeField] private bool randomRotation = true;
    [Space]
    [SerializeField] private bool randomOffset = true;
    [SerializeField] private float xMinOffset = -0.3f;
    [SerializeField] private float xMaxOffset = 0.3f;
    [Space]
    [SerializeField] private float yMinOffset = -0.3f;
    [SerializeField] private float yMaxOffset = 0.3f;

    private Coroutine destroyObjectCoroutine;

    private void Start()
    {
        ApplyRandomOffset();
        ApplyRandomRotation();

        if (autoDestroy)
            Destroy(gameObject, destroyInSecs);

        //if (destroyObjectCoroutine != null)
        //    StopCoroutine(destroyObjectCoroutine);

        //destroyObjectCoroutine = StartCoroutine(DestroyGameObject());
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(destroyInSecs);
        Destroy(gameObject);
    }

    private void ApplyRandomOffset()
    {
        if (randomOffset == false)
            return;

        float xOffset = Random.Range(xMinOffset, xMaxOffset);
        float yOffset = Random.Range(yMinOffset, yMaxOffset);

        transform.position = transform.position + new Vector3(xOffset, yOffset);
    }

    private void ApplyRandomRotation()
    {
        if (randomRotation == false)
            return;

        float zRotation = Random.Range(0, 360);
        transform.Rotate(0, 0, zRotation);
    }
}
