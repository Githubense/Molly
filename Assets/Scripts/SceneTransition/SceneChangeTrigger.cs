using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChangeTrigger : MonoBehaviour
{
    public string sceneToLoad;
    public Vector3 spawnPosition;
    private static bool isChangingScene = false; // Flag to prevent immediate changes
    [SerializeField] private bool requiresInitialInteraction = false; // Add this line

    private void Start()
    {
        StartCoroutine(EnableSceneChangeAfterDelay());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isChangingScene)
        {
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

    private IEnumerator ChangeScene()
    {
        yield return SceneManager.LoadSceneAsync(sceneToLoad);

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
