using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageSlide : MonoBehaviour
{
    [Header("Posizione nascosta (fuori schermo)")]
    [SerializeField] private Vector2 hiddenPosition = new Vector2(0, -600);

    [Header("Posizione visibile (dentro lo schermo)")]
    [SerializeField] private Vector2 visiblePosition = new Vector2(0, 0);

    [Header("Durata animazione (in secondi)")]
    [SerializeField] private float slideDuration = 1f;

    [Header("Mostra e poi nascondi automaticamente all'avvio?")]
    [SerializeField] private bool showAndHideOnStart = true;

    [Header("Tempo di permanenza visibile (se showAndHideOnStart = true)")]
    [SerializeField] private float visibleTime = 2f;

    private RectTransform targetRect; 
    private Coroutine slideRoutine;

    private void Awake()
    {
        targetRect = GetComponent<RectTransform>();
        if (targetRect != null)
        {
            // Parto in hiddenPosition (in basso)
            targetRect.anchoredPosition = hiddenPosition;
        }
    }

    private void Start()
    {
        if (showAndHideOnStart)
        {
            StartCoroutine(ShowHideSequence());
        }
    }

    // --- Metodi pubblici se vuoi richiamarli manualmente ---
    public void ShowImage()
    {
        if (slideRoutine != null) StopCoroutine(slideRoutine);
        slideRoutine = StartCoroutine(SlideAnimation(hiddenPosition, visiblePosition));
    }

    public void HideImage()
    {
        if (slideRoutine != null) StopCoroutine(slideRoutine);
        slideRoutine = StartCoroutine(SlideAnimation(visiblePosition, hiddenPosition));
    }

    // --- Sequence automatica: entra -> aspetta visibleTime -> esce
    private IEnumerator ShowHideSequence()
    {
        // 1) Entra
        ShowImage();
        // Aspetto fine dell'animazione
        yield return new WaitWhile(() => slideRoutine != null);

        // 2) Resto X secondi
        yield return new WaitForSeconds(visibleTime);

        // 3) Esco
        HideImage();
        yield return new WaitWhile(() => slideRoutine != null);

        // (Se vuoi, disattiva l'oggetto)
        // gameObject.SetActive(false);
    }

    // --- Coroutine generica di animazione ---
    private IEnumerator SlideAnimation(Vector2 startPos, Vector2 endPos)
    {
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);

            // Lerp dalla pos iniziale alla finale
            if (targetRect != null)
            {
                targetRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            }

            yield return null;
        }

        // Fine: assicura la posizione finale
        if (targetRect != null)
        {
            targetRect.anchoredPosition = endPos;
        }

        slideRoutine = null;
    }
}
