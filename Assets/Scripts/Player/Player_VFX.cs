using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    private Coroutine imageEchoCoroutine;

    [Header("Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float imageEchoInterval = 0.05f;
    [SerializeField] private GameObject echoVFX;

    public void CreateImageEchoEffect(float duration)
    {
        if (imageEchoCoroutine != null)
            StopCoroutine(imageEchoCoroutine);

        imageEchoCoroutine = StartCoroutine(ImageEchoCo(duration));
    }

    private IEnumerator ImageEchoCo(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            CreateImageEcho();

            yield return new WaitForSeconds(imageEchoInterval);
            time = time + imageEchoInterval;
        }
    }

    private void CreateImageEcho()
    {
        GameObject imageEcho = Instantiate(echoVFX, transform.position, transform.rotation);

        VFX_Controller vfxController = imageEcho.GetComponent<VFX_Controller>();
        vfxController.SetRendererSprite(spriteRenderer.sprite);
    }
}
