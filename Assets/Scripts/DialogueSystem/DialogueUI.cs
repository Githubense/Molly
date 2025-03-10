using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueUi : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;

    public bool isOpen { get; private set; }

    private TypeEffect typeEffect;
    
    // UnityEvent to notify when dialogue is closed (optional)
    public UnityEvent OnDialogueClosed;

    private void Start()
    {
        typeEffect = GetComponent<TypeEffect>();
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        // Always allow opening of the dialogue
        isOpen = true;
        dialogueBox.SetActive(true);

        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        // Step through each line
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogueLine = dialogueObject.Dialogue[i];
            yield return typeEffect.Run(dialogueLine, textLabel);

            // Wait for user to press Space before showing the next line
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        // After the last line, pressing Space again will close
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        CloseDialogueBox();
    }

    private void CloseDialogueBox()
    {
        isOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        OnDialogueClosed?.Invoke();
    }
}
