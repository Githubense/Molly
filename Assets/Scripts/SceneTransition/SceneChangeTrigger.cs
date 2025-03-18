using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChangeTrigger : MonoBehaviour
{
    public string sceneToLoad;
    public Vector3 spawnPosition;
    private static bool isChangingScene = false; // Flag per evitare cambi immediati

    private void Start()
    {
        StartCoroutine(EnableSceneChangeAfterDelay());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isChangingScene)
        {
            isChangingScene = true; // Blocca il cambio scena per un po'
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

        yield return new WaitForSeconds(1f); // Attendi 1 secondo prima di riattivare il cambio scena
        isChangingScene = false;
    }

    private IEnumerator EnableSceneChangeAfterDelay()
    {
        isChangingScene = true;
        yield return new WaitForSeconds(1f);
        isChangingScene = false;
    }
}
