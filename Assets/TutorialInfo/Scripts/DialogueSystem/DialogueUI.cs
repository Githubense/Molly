using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueUi : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject testDialogue; // optional reference in Inspector

    public bool isOpen { get; private set; }

    private TypeEffect typeEffect;

    // UnityEvent to notify when dialogue is closed
    public UnityEvent OnDialogueClosed;

    private void Start()
    {
        typeEffect = GetComponent<TypeEffect>();
        CloseDialogueBox();
    }

    // Remove the 'hasResponded' checks so that ShowDialogue can be called infinitely
    public void ShowDialogue(DialogueObject dialogueObject)
    {
        // Always allow opening
        isOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogueLine = dialogueObject.Dialogue[i];
            yield return typeEffect.Run(dialogueLine, textLabel);

            // Wait for user to press "Space" before showing next line
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        // Once all lines are done, pressing Space once more closes the dialogue
        CloseDialogueBox();
    }

    private void CloseDialogueBox()
    {
        isOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;

        // Trigger the event when the dialogue is closed
        OnDialogueClosed?.Invoke();
    }
}
