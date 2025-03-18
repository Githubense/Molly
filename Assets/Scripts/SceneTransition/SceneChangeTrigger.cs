using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;     // Nome della scena di destinazione
    [SerializeField] private Vector3 spawnPosition;  // Posizione del Player nella nuova scena
    [SerializeField] private CanvasFade canvasFade;  // Assegna in Inspector il CanvasFade della vecchia scena

    private bool isChangingScene = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isChangingScene && other.CompareTag("Player"))
        {
            isChangingScene = true;
            StartCoroutine(ChangeSceneRoutine());
        }
    }

    private IEnumerator ChangeSceneRoutine()
    {
        // 1) Fade out (vecchia scena)
        if (canvasFade != null)
        {
            yield return StartCoroutine(canvasFade.FadeOutRoutine());
        }

        // 2) Carico la nuova scena
        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        // 3) Trovo il Player nella nuova scena e lo posiziono
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = spawnPosition;
        }

        isChangingScene = false;
    }
}
