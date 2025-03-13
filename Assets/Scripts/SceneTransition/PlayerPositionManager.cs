using UnityEngine;
using System.Collections.Generic;


    public class PlayerPositionManager : MonoBehaviour
    {
        public static PlayerPositionManager Instance;

        // Dizionario per memorizzare la posizione del giocatore per ogni scena
        private Dictionary<string, Vector3> scenePlayerPositions = new Dictionary<string, Vector3>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Evita che venga distrutto tra le scene
            }
            else
            {
                Destroy(gameObject); // Mantieni solo una copia di questo oggetto
            }
        }

        // Salva la posizione del giocatore per una scena specifica
        public void SavePlayerPosition(string sceneName, Vector3 position)
        {
            if (scenePlayerPositions.ContainsKey(sceneName))
            {
                scenePlayerPositions[sceneName] = position;
            }
            else
            {
                scenePlayerPositions.Add(sceneName, position);
            }
        }

        // Carica la posizione del giocatore per una scena specifica
        public Vector3 LoadPlayerPosition(string sceneName)
        {
            if (scenePlayerPositions.ContainsKey(sceneName))
            {
                return scenePlayerPositions[sceneName];
            }

            // Se non c'è una posizione salvata per quella scena, restituisci una posizione di default (0,0,0)
            return Vector3.zero;
        }
    }
