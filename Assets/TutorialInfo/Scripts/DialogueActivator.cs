using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private GameObject visualCue;  // Reference to the visual cue object

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
        {
            player.Interactable = this;

            // Enable the visual cue when player is near
            if (visualCue != null)
                visualCue.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
        {
            if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this) // Fixed comparison
            {
                player.Interactable = null;
                
                // Disable the visual cue when player leaves
                if (visualCue != null)
                    visualCue.SetActive(false);
            }
        }
    }

    public void Interact(PlayerMovement player)
    {
        player.DialogueUi.ShowDialogue(dialogueObject);
    }
}
