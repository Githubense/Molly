using System;
using System.Collections;
using UnityEngine;
using TMPro;


public class DialogueUi : MonoBehaviour
{

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;

    [SerializeField] private DialogueObject testDialogue;

    public bool isOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypeEffect typeEffect;

    private void Start()
    {
        typeEffect = GetComponent<TypeEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        CloseDialogueBox();
    }
    public void ShowDialogue(DialogueObject dialogueObject)
    {
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

        // Check if we are at the last dialogue and if it has responses.
        if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.Responses != null && dialogueObject.HasResponses) 
        {
            break; // Exit loop to show responses after the last dialogue step
        }

        // Wait for the player to press space before continuing
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
    }

    if (dialogueObject.HasResponses) 
    {
        // Show responses
        responseHandler.ShowResponses(dialogueObject.Responses);

        // Wait for the player to press space after responses to close the dialogue box
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        // After pressing space, close the dialogue box
        CloseDialogueBox();
    }
    else 
    {
        // No responses, just close the dialogue box
        CloseDialogueBox();
    }
}
        private void CloseDialogueBox()
        {
            isOpen = false;
            dialogueBox.SetActive(false);
            textLabel.text = string.Empty;
        }
    }
