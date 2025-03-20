using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private CanvasFade canvasFade;
    [SerializeField] private bool requiresInitialInteraction = false;

    private static bool isChangingScene = false;

    private void Start()
    {
        StartCoroutine(EnableSceneChangeAfterDelay());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isChangingScene)
        {
            if (requiresInitialInteraction && !StorylineManager.Instance.IsInitialInteractionCompleted())
            {
                return;
            }

            isChangingScene = true;
            PlayerPositionManager.Instance.SavePlayerPosition(SceneManager.GetActiveScene().name, other.transform.position);
            StartCoroutine(ChangeSceneRoutine());
        }
    }

    private IEnumerator ChangeSceneRoutine()
    {
        if (canvasFade != null)
        {
            yield return canvasFade.StartCoroutine(canvasFade.FadeOutRoutine());
        }

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = spawnPosition;
        }

        yield return new WaitForSeconds(1f);
        isChangingScene = false;
    }

    private IEnumerator EnableSceneChangeAfterDelay()
    {
        isChangingScene = true;
        yield return new WaitForSeconds(1f);
        isChangingScene = false;
    }
}
