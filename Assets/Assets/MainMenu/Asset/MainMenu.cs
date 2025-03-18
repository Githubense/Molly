using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "Test"; // Nome della scena di gioco
    public GameObject settingsCanvas; // Canvas delle impostazioni

    void Start()
    {
        if (settingsCanvas != null)
        {
            settingsCanvas.SetActive(false); // Nasconde il canvas delle impostazioni all'avvio
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Gioco chiuso!"); // Utile per il debug, dato che Application.Quit() non funziona nell'editor
    }

    public void ToggleSettings()
    {
        if (settingsCanvas != null)
        {
            settingsCanvas.SetActive(!settingsCanvas.activeSelf); // Mostra/nasconde il canvas
        }
    }
}
