using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private CanvasFade canvasFade; 
    // assegni in Inspector l'oggetto col CanvasFade (quello che fa fade in/out)

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
        // 1) Fade out (vecchia scena: 0->1)
        if (canvasFade != null)
        {
            yield return canvasFade.StartCoroutine(canvasFade.FadeOutRoutine());
        }

        // 2) Carico la nuova scena
        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        // 3) Posiziono il Player
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = spawnPosition;
        }

        isChangingScene = false;
    }
}
