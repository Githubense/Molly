using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "Test"; // Nome della scena di gioco
    public GameObject settingsPanel; // Pannello delle impostazioni

    void Start()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // Nasconde le impostazioni all'avvio
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
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf); // Mostra/nasconde il pannello
        }
    }
}
