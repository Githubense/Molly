using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.EventSystems;

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
    private InputAction interactAction;

    private int currentChoiceIndex;
    private bool choiceSelected;
    private bool displayingResult; // Flag to indicate if displaying result
    private bool canSelectChoice; // Flag to indicate if choices can be selected

    private void Awake()
    {
        skipDialogueAction = new InputAction(binding: "<Keyboard>/space");
        navigateAction = new InputAction(binding: "<Gamepad>/dpad");
        selectAction = new InputAction(binding: "<Gamepad>/buttonSouth");
        interactAction = new InputAction(binding: "<Gamepad>/buttonEast");
    }

    private void OnEnable()
    {
        skipDialogueAction.Enable();
        navigateAction.Enable();
        selectAction.Enable();
        interactAction.Enable();
    }

    private void OnDisable()
    {
        skipDialogueAction.Disable();
        navigateAction.Disable();
        selectAction.Disable();
        interactAction.Disable();
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
            yield return new WaitUntil(() => skipDialogueAction.triggered || interactAction.triggered);
        }

        if (dialogueObject.HasChoices)
        {
            ShowChoices(dialogueObject.ChoiceKeys);
            yield return new WaitForSeconds(0.5f); // Add a delay before allowing selection
            canSelectChoice = true;
            yield return new WaitUntil(() => choiceSelected);
            HandleChoiceSelection(dialogueObject);
        }
        else
        {
            CloseDialogueBox();
        }
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

                // Add event listeners for mouse and touch input
                int choiceIndex = i;
                EventTrigger trigger = choiceLabels[i].gameObject.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
                entry.callback.AddListener((eventData) => OnChoiceSelected(choiceIndex));
                trigger.triggers.Add(entry);
            }
        }
        // Hide any unused choice labels
        for (int i = choiceKeys.Length; i < choiceLabels.Length; i++)
        {
            choiceLabels[i].gameObject.SetActive(false);
        }
        currentChoiceIndex = -1; // Set to -1 to indicate no selection
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
        displayingResult = false; // Reset the displayingResult flag
        canSelectChoice = false; // Reset the canSelectChoice flag
    }

    private void Update()
    {
        if (!isOpen) return;

        if (navigateAction.triggered && canSelectChoice)
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

        if (selectAction.triggered && currentChoiceIndex >= 0 && canSelectChoice)
        {
            // Trigger the selection action
            OnChoiceSelected(currentChoiceIndex);
        }

        // Allow dismissing the dialogue canvas with spacebar, east button, or B button
        if (skipDialogueAction.triggered || interactAction.triggered || Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            if (displayingResult)
            {
                CloseDialogueBox();
            }
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        currentChoiceIndex = choiceIndex;
        choiceSelected = true;
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

        // Save the selection in the StorylineManager
        StorylineManager.Instance.SetInteractionState(dialogueObject.ChoiceKeys[currentChoiceIndex], true);

        // Hide the choice box to prevent changing the decision
        choiceBox.SetActive(false);
    }

    private IEnumerator DisplayResult(string resultLine)
    {
        displayingResult = true; // Set the flag to indicate result is being displayed
        yield return typeEffect.Run(resultLine, textLabel);
        yield return new WaitUntil(() => skipDialogueAction.triggered || interactAction.triggered || Gamepad.current.buttonWest.wasPressedThisFrame);
        CloseDialogueBox();
    }
}