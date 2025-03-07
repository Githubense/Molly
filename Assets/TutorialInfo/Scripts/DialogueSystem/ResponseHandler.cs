using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    private DialogueUi dialogueUI;
    private DialogueActivator dialogueActivator;  // Reference to the DialogueActivator

    private List<Button> tempResponseButton = new List<Button>(); // Store only the buttons
    private bool hasResponded = false; // Track if a response has been selected

    private void Start()
    {
        dialogueUI = GetComponent<DialogueUi>();
        dialogueActivator = GetComponent<DialogueActivator>(); // Get the DialogueActivator reference
    }

    public void ShowResponses(Response[] responses)
    {
        if (hasResponded) return; // If a response has already been selected, do nothing

        float responseBoxHeight = 0;

        foreach (Response response in responses)
        {
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            Button buttonComponent = responseButton.GetComponent<Button>();

            // Add click listener only if no response has been given yet
            buttonComponent.onClick.AddListener(() => OnPickedResponse(response));

            // Add the button to the list
            tempResponseButton.Add(buttonComponent);

            // Calculate the height of the response box
            responseBoxHeight += responseButtonTemplate.sizeDelta.y;
        }

        // Set the dynamic height for the response box
        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);

        // Make the response box visible
        responseBox.gameObject.SetActive(true);
    }

    private void OnPickedResponse(Response response)
    {
        if (hasResponded) return; // If a response has already been chosen, do nothing

        hasResponded = true; // Mark that a response has been chosen

        // Hide the response box after the selection
        responseBox.gameObject.SetActive(false);

        // Disable further interaction by removing all listeners from the buttons
        foreach (Button button in tempResponseButton)
        {
            button.onClick.RemoveAllListeners(); // Remove all event listeners
        }

        // Clear the list of buttons (if not used anymore)
        tempResponseButton.Clear();

        // Hide the visual cue and disable interaction permanently
        if (dialogueActivator != null)
        {
            dialogueActivator.DisableInteraction();  // Disable interaction and hide visual cue
        }

        // Show the dialogue associated with the response
        dialogueUI.ShowDialogue(response.DialogueObject);
    }
}
