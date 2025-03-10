using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private GameObject visualCue;

    private bool canInteract = true;  // Controls if this object can still be interacted with

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
        {
            // Only show the visual cue & assign 'Interactable' if we have not yet interacted
            if (canInteract)
            {
                player.Interactable = this;
                if (visualCue != null) 
                    visualCue.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
        {
            // Clear the player's Interactable
            if (player.Interactable == this)
                player.Interactable = null;

            // Hide the visual cue
            if (visualCue != null)
                visualCue.SetActive(false);
        }
    }

    // Called from the player's code when pressing the interact key (e.g. E)
    public void Interact(PlayerMovement player)
    {
        // If canInteract is already false, do nothing
        if (!canInteract) return;

        // Show the dialogue for the first time
        player.DialogueUi.ShowDialogue(dialogueObject);

        // After the first interaction, set canInteract to false
        canInteract = false;

        // Hide the visual cue (so it doesn't mislead the user)
        if (visualCue != null)
            visualCue.SetActive(false);
    }
}
