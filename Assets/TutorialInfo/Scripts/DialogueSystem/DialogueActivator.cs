using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private GameObject visualCue;

    // Always let player interact
    private bool canInteract = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player) && canInteract)
        {
            player.Interactable = this;
            if (visualCue != null) visualCue.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
        {
            player.Interactable = null;
            if (visualCue != null) visualCue.SetActive(false);
        }
    }

    public void Interact(PlayerMovement player)
    {
        // Show dialogue UI; no one-time blocking
        player.DialogueUi.ShowDialogue(dialogueObject);
    }

    // Remove or comment out permanent disabling
    // public void DisableInteraction()
    // {
    //     canInteract = false;
    //     if (visualCue != null)
    //         visualCue.SetActive(false);
    // }
}
