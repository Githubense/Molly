using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove_Ref : MonoBehaviour
{
    public int sceneBuildIndex;
    private bool canTeleport = false;

    private void Start()
    {
        // Attende un secondo prima di riabilitare il teletrasporto
        Invoke("EnableTeleport", 1f);
    }

    private void EnableTeleport()
    {
        canTeleport = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canTeleport)
        {
            // Salva la posizione prima di cambiare scena
            Vector3 playerPosition = other.transform.position;
            PlayerPositionManager.Instance.SavePosition(SceneManager.GetActiveScene().buildIndex, playerPosition);
            
            // Cambia scena
            SceneManager.LoadScene(sceneBuildIndex);
        }
    }
}
