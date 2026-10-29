using System.Collections;
using UnityEngine;

public class FadeCode : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public float fadeDuration = 1f;

    public void Awake()
    {
        if (CanvasGroup == null)
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }
    }

    public IEnumerator FadeOut()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time+= Time.deltaTime;
            CanvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            yield return null;
        }
    }


    public IEnumerator FadeIn()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            CanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }
    }
}
