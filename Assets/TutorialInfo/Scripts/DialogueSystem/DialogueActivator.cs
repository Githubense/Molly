using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private GameObject visualCue;

    private bool canInteract = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player) && canInteract)
        {
            player.Interactable = this;

            // Enable visual cue
            if (visualCue != null)
                visualCue.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
        {
            player.Interactable = null;
            // Hide visual cue when player leaves
            if (visualCue != null)
                visualCue.SetActive(false);
        }
    }

    public void Interact(PlayerMovement player)
    {
        // Show dialogue UI
        player.DialogueUi.ShowDialogue(dialogueObject);
    }

    // Disable interaction permanently (e.g., for one-time interactions)
    public void DisableInteraction()
    {
        canInteract = false;
        if (visualCue != null)
            visualCue.SetActive(false);
    }
}
