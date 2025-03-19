using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private GameObject visualCue;
    [SerializeField] private string interactionKey;

    private bool canInteract = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
        {
            if (canInteract && StorylineManager.Instance.CanInteract(interactionKey))
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
            if (player.Interactable == this)
                player.Interactable = null;

            if (visualCue != null)
                visualCue.SetActive(false);
        }
    }

    public void Interact(PlayerMovement player)
    {
        if (!canInteract || !StorylineManager.Instance.CanInteract(interactionKey)) return;

        player.DialogueUi.ShowDialogue(dialogueObject);
        StorylineManager.Instance.SetInteractionState(interactionKey, true);
        canInteract = false;

        if (visualCue != null)
            visualCue.SetActive(false);
    }
}
