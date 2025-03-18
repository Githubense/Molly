using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnEnable : MonoBehaviour
{
    public string sceneToLoad; // Imposta la scena nell'Inspector

    private void OnEnable()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Nessuna scena specificata!");
        }
    }
}
