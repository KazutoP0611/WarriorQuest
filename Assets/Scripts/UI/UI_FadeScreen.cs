using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeScreen : MonoBehaviour
{
    private Slider slider;

    public Coroutine fadeCoroutine { get; private set; }

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    public void FadeIn(float fadeInSecs = 0.5f)
    {
        slider.value = 0;
        DoFade(1, fadeInSecs);
    }

    public void FadeOut(float fadeInSecs = 0.5f)
    {
        slider.value = 1;
        DoFade(0, fadeInSecs);
    }

    private void DoFade(float targetValue, float fadeInSecs)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeEffectCo(targetValue, fadeInSecs));
    }

    private IEnumerator FadeEffectCo(float targetValue, float fadeInSecs)
    {
        float startValue = slider.value;
        float elapsedTime = 0;

        while (elapsedTime < fadeInSecs)
        {
            elapsedTime += Time.deltaTime;
            float fillAmount = Mathf.Lerp(startValue, targetValue, elapsedTime / fadeInSecs);
            slider.value = fillAmount;

            yield return null;
        }

        slider.value = targetValue;
    }
}
