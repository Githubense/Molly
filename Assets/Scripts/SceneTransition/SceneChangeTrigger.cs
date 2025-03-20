using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private CanvasFade canvasFade; 
    // assegni in Inspector l'oggetto col CanvasFade (quello che fa fade in/out)
    public string sceneToLoad;
    public Vector3 spawnPosition;
    private static bool isChangingScene = false; // Flag to prevent immediate changes
    [SerializeField] private bool requiresInitialInteraction = false; // Add this line

    private bool isChangingScene = false;
    private void Start()
    {
        StartCoroutine(EnableSceneChangeAfterDelay());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isChangingScene && other.CompareTag("Player"))
        if (other.CompareTag("Player") && !isChangingScene)
        {
            isChangingScene = true;
            StartCoroutine(ChangeSceneRoutine());
            // Check if the required interaction has been completed
            if (requiresInitialInteraction && !StorylineManager.Instance.HasInteracted("Computer"))
            {
                return;
            }

            isChangingScene = true; // Block scene change for a while
            PlayerPositionManager.Instance.SavePlayerPosition(SceneManager.GetActiveScene().name, other.transform.position);
            StartCoroutine(ChangeScene());
        }
    }

    private IEnumerator ChangeSceneRoutine()
    private IEnumerator ChangeScene()
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

        yield return new WaitForSeconds(1f); // Wait 1 second before re-enabling scene change
        isChangingScene = false;
    }

    private IEnumerator EnableSceneChangeAfterDelay()
    {
        isChangingScene = true;
        yield return new WaitForSeconds(1f);
        isChangingScene = false;
    }
}
