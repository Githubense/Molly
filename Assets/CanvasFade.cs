using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasFade : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup; 
    // Assegna in Inspector il CanvasGroup (o metti "GetComponent<CanvasGroup>()" in Awake())

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;    // Quanto dura il fade
    [SerializeField] private bool fadeInOnStart = true;  // Se true, la scena parte nera e fa il fade in automatico

    private void Awake()
    {
        // Se non l'hai già assegnato in Inspector
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // Se vogliamo avviare la scena in nero
        if (fadeInOnStart)
        {
            canvasGroup.alpha = 1f;  // nero totale
        }
        else
        {
            canvasGroup.alpha = 0f;  // trasparente
        }
    }

    private void Start()
    {
        // Se la scena deve partire nera e schiarirsi
        if (fadeInOnStart)
        {
            StartCoroutine(FadeInRoutine());
        }
    }

    /// <summary>
    /// Dal nero (alpha=1) a trasparente (alpha=0).
    /// </summary>
    public IEnumerator FadeInRoutine()
    {
        float startAlpha = canvasGroup.alpha; // dovrebbe essere 1
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }

        canvasGroup.alpha = 0f; // fine fade in
    }

    /// <summary>
    /// Dalla trasparenza (alpha=0) al nero (alpha=1).
    /// </summary>
    public IEnumerator FadeOutRoutine()
    {
        float startAlpha = canvasGroup.alpha; // di solito 0
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, t);
            yield return null;
        }

        canvasGroup.alpha = 1f; // fine fade out
    }
}
