using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasFade : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private bool fadeInOnStart = true; // se true, parte nero e fa fade in

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // Se voglio partire nero, metto alpha=1
        if (fadeInOnStart)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }

    private void Start()
    {
        // Se ho segnato fadeInOnStart, avvio una Coroutine che fa alpha=1 -> 0
        if (fadeInOnStart)
        {
            StartCoroutine(FadeInRoutine());
        }
    }

    public IEnumerator FadeInRoutine()
    {
        float startAlpha = canvasGroup.alpha; // di solito 1
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }

    public IEnumerator FadeOutRoutine()
    {
        float startAlpha = canvasGroup.alpha; // di solito 0
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
}
