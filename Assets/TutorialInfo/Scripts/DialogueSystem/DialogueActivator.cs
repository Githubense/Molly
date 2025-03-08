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
            if (visualCue != null && !player.HasInteracted) // Only show if not interacted already
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
            }
        }
    }

    public void Interact(PlayerMovement player)
    {
        // Show dialogue when the player interacts
        player.DialogueUi.ShowDialogue(dialogueObject);

        // Mark as interacted
        player.HasInteracted = true;

        // Subscribe to the event for when the dialogue closes
        player.DialogueUi.OnDialogueClosed.AddListener(HideVisualCue);
    }

    private void HideVisualCue()
    {
        // Hide the visual cue after the dialogue is closed
        if (visualCue != null)
        {
            visualCue.SetActive(false);
        }

        // Optionally, unsubscribe if you don't need further updates after this
        // player.DialogueUi.OnDialogueClosed.RemoveListener(HideVisualCue);
    }

    // Disable interaction and hide the visual cue permanently
    public void DisableInteraction()
    {
        canInteract = false;

        // Hide the visual cue permanently if needed
        if (visualCue != null)
        {
            visualCue.SetActive(false);
        }
    }
}
