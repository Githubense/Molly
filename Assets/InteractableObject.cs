using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public void Interact()
    {
        Debug.Log("Hai interagito con " + gameObject.name);
        // Aggiungi qui la logica dell'oggetto (es. aprire una porta, parlare con un NPC, raccogliere un oggetto)
    }
}
