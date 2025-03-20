using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private string interactionKey;
    [SerializeField] private bool requiresPreviousInteraction = false;
    [SerializeField] private string previousInteractionKey;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
        {
            if (requiresPreviousInteraction && !StorylineManager.Instance.HasInteracted(previousInteractionKey))
            {
                return;
            }

            player.DialogueUi.ShowDialogue(dialogueObject);
            StorylineManager.Instance.SetInteractionState(interactionKey, true);
        }
    }
}
