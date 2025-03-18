using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class DialogueUi : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;

    public bool isOpen { get; private set; }

    private TypeEffect typeEffect;
    public UnityEvent OnDialogueClosed;

    private InputAction skipDialogueAction;

    private void Awake()
    {
        skipDialogueAction = new InputAction(binding: "<Keyboard>/space");
    }

    private void OnEnable()
    {
        skipDialogueAction.Enable();
    }

    private void OnDisable()
    {
        skipDialogueAction.Disable();
    }

    private void Start()
    {
        typeEffect = GetComponent<TypeEffect>();
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
        for (int i = 0; i < dialogueObject.DialogueKeys.Length; i++)
        {
            string dialogueKey = dialogueObject.DialogueKeys[i];
            var localizedString = new LocalizedString("DialogueTable", dialogueKey);
            string dialogueLine = localizedString.GetLocalizedString();

            yield return typeEffect.Run(dialogueLine, textLabel);
            yield return new WaitUntil(() => skipDialogueAction.triggered);
        }

        yield return new WaitUntil(() => skipDialogueAction.triggered);
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