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
    [SerializeField] private GameObject choiceBox;
    [SerializeField] private TMP_Text[] choiceLabels;

    public bool isOpen { get; private set; }

    private TypeEffect typeEffect;
    public UnityEvent OnDialogueClosed;

    private InputAction skipDialogueAction;
    private InputAction navigateAction;
    private InputAction selectAction;

    private int currentChoiceIndex;
    private bool choiceSelected;

    private void Awake()
    {
        skipDialogueAction = new InputAction(binding: "<Keyboard>/space");
        navigateAction = new InputAction(binding: "<Gamepad>/dpad");
        selectAction = new InputAction(binding: "<Gamepad>/buttonSouth");
    }

    private void OnEnable()
    {
        skipDialogueAction.Enable();
        navigateAction.Enable();
        selectAction.Enable();
    }

    private void OnDisable()
    {
        skipDialogueAction.Disable();
        navigateAction.Disable();
        selectAction.Disable();
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

        if (dialogueObject.HasChoices)
        {
            ShowChoices(dialogueObject.ChoiceKeys);
            yield return new WaitUntil(() => choiceSelected);
            HandleChoiceSelection(dialogueObject);
        }

        CloseDialogueBox();
    }

    private void ShowChoices(string[] choiceKeys)
    {
        choiceBox.SetActive(true);
        for (int i = 0; i < choiceKeys.Length; i++)
        {
            if (i < choiceLabels.Length)
            {
                var localizedString = new LocalizedString("DialogueTable", choiceKeys[i]);
                choiceLabels[i].text = localizedString.GetLocalizedString();
                choiceLabels[i].gameObject.SetActive(true);
            }
        }
        // Hide any unused choice labels
        for (int i = choiceKeys.Length; i < choiceLabels.Length; i++)
        {
            choiceLabels[i].gameObject.SetActive(false);
        }
        currentChoiceIndex = 0;
        UpdateChoiceSelection();
    }

    private void UpdateChoiceSelection()
    {
        for (int i = 0; i < choiceLabels.Length; i++)
        {
            choiceLabels[i].color = i == currentChoiceIndex ? Color.yellow : Color.white;
        }
    }

    private void CloseDialogueBox()
    {
        isOpen = false;
        dialogueBox.SetActive(false);
        choiceBox.SetActive(false);
        textLabel.text = string.Empty;
        OnDialogueClosed?.Invoke();
        choiceSelected = false; // Reset the choiceSelected flag
    }

    private void Update()
    {
        if (!isOpen) return;

        if (navigateAction.triggered)
        {
            Vector2 navigation = navigateAction.ReadValue<Vector2>();
            if (navigation.y > 0)
            {
                currentChoiceIndex = Mathf.Max(currentChoiceIndex - 1, 0);
            }
            else if (navigation.y < 0)
            {
                currentChoiceIndex = Mathf.Min(currentChoiceIndex + 1, choiceLabels.Length - 1);
            }
            UpdateChoiceSelection();
        }

        if (selectAction.triggered)
        {
            // Trigger the selection action
            choiceSelected = true;
        }
    }

    private void HandleChoiceSelection(DialogueObject dialogueObject)
    {
        string choiceResultKey = dialogueObject.GetChoiceResultKey(currentChoiceIndex);
        if (!string.IsNullOrEmpty(choiceResultKey))
        {
            var localizedString = new LocalizedString("DialogueTable", choiceResultKey);
            string resultLine = localizedString.GetLocalizedString();
            StartCoroutine(DisplayResult(resultLine));
        }
    }

    private IEnumerator DisplayResult(string resultLine)
    {
        yield return typeEffect.Run(resultLine, textLabel);
        yield return new WaitUntil(() => skipDialogueAction.triggered);
        CloseDialogueBox();
    }
}