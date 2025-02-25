using UnityEngine;
using System.Collections.Generic;

public class PlayerPositionManager : MonoBehaviour
{
    public static PlayerPositionManager Instance;
    private Dictionary<int, Vector3> scenePositions = new Dictionary<int, Vector3>();

    private void Awake()
    {
        // Assicura che esista solo un'istanza di questo oggetto (Singleton)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePosition(int sceneIndex, Vector3 position)
    {
        if (scenePositions.ContainsKey(sceneIndex))
        {
            scenePositions[sceneIndex] = position;
        }
        else
        {
            scenePositions.Add(sceneIndex, position);
        }
    }

    public Vector3 GetSavedPosition(int sceneIndex)
    {
        if (scenePositions.ContainsKey(sceneIndex))
        {
            return scenePositions[sceneIndex];
        }

        // Se non c'è una posizione salvata, restituisce Vector3.zero (spawn di default)
        return Vector3.zero;
    }
}
