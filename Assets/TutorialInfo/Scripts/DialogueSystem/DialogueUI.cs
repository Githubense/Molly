using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueUi : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject testDialogue;

    public bool isOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypeEffect typeEffect;

    private bool hasResponded = false;  // Track if a response has been given

    // UnityEvent to notify when dialogue is closed
    public UnityEvent OnDialogueClosed;

    private void Start()
    {
        typeEffect = GetComponent<TypeEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        if (hasResponded) return;  // If a response was already provided, do nothing

        isOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typeEffect.Run(dialogue, textLabel);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.Responses != null && dialogueObject.HasResponses)
            {
                break; // Exit to show responses
            }

            // Wait for the user to press "Space" to continue
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);

            // Wait for the user to press "Space" after the responses to close the dialogue box
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            CloseDialogueBox();

            // Mark that the user has responded
            hasResponded = true;
        }
        else
        {
            CloseDialogueBox();
        }
    }

    private void CloseDialogueBox()
    {
        isOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;

        // Trigger the event when the dialogue is closed
        OnDialogueClosed?.Invoke();
    }

    public void ResetResponseStatus()
    {
        // Reset the 'hasResponded' flag to allow interaction with this dialogue again
        hasResponded = false;
    }
}
