using UnityEngine;
using UnityEngine.SceneManagement;

    public class PlayerStartPosition : MonoBehaviour
    {
        private void Start()
        {
            // Carica la posizione del giocatore per la scena corrente
            string currentSceneName = SceneManager.GetActiveScene().name;
            transform.position = PlayerPositionManager.Instance.LoadPlayerPosition(currentSceneName);
        }
    }
