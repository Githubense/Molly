using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private GameObject visualCue;  // Reference to the visual cue object

    private bool canInteract = true;  // Flag to track whether interaction is allowed

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player) && canInteract)
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
            // Only disable interaction if the player was interacting with this specific DialogueActivator
            if (player.Interactable == this)
            {
                player.Interactable = null;

                // Disable the visual cue when player leaves the trigger zone
                if (visualCue != null)
                    visualCue.SetActive(false);
            }
        }
    }

    public void Interact(PlayerMovement player)
    {
        player.DialogueUi.ShowDialogue(dialogueObject);
    }

    // Disable interaction and hide the visual cue
    public void DisableInteraction()
    {
        canInteract = false;

        // Hide the visual cue permanently
        if (visualCue != null)
        {
            visualCue.SetActive(false);
        }
    }
}
