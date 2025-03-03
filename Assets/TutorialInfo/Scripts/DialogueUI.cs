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
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject) 
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typeEffect.Run(dialogue, textLabel);

            if(i == dialogueObject.Dialogue.Length - 1 && dialogueObject.Responses != null && dialogueObject.HasResponses) break;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if(dialogueObject.HasResponses) 
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
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
        }
    }
