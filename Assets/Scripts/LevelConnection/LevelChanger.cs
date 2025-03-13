using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField]
    private LevelConnection _connection;

    [SerializeField]
    private string _targetSceneName;

    [SerializeField]
    private Transform _spawnPoint;

    private void Start()
    {
        if (_connection == LevelConnection.ActiveConnection)
        {
            // Usa FindFirstObjectByType per trovare il primo oggetto di tipo PlayerMovement
            PlayerMovement player = Object.FindFirstObjectByType<PlayerMovement>();
            if (player != null)
            {
                player.transform.position = _spawnPoint.position;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.collider.GetComponent<PlayerMovement>();
        if (player != null)
        {
            LevelConnection.ActiveConnection = _connection;
            SceneManager.LoadScene(_targetSceneName);
        }
    }
}
