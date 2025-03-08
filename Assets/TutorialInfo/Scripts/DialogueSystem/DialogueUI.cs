using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;  // Add this for UnityEvent

public class DialogueUi : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;

    [SerializeField] private DialogueObject testDialogue;

    public bool isOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypeEffect typeEffect;

    private bool hasResponded = false;  // Traccia se la risposta è stata fornita

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
        if (hasResponded) return;  // Se la risposta è già stata fornita, non fare nulla

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

            // Se siamo all'ultimo messaggio e ci sono risposte, fermiamo il ciclo
            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.Responses != null && dialogueObject.HasResponses) 
            {
                break; // Uscita per mostrare le risposte
            }

            // Aspetta che l'utente prema "Space" per andare avanti
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if (dialogueObject.HasResponses) 
        {
            // Mostra le risposte
            responseHandler.ShowResponses(dialogueObject.Responses);

            // Aspetta che l'utente prema "Space" dopo le risposte per chiudere la finestra del dialogo
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            // Dopo che l'utente ha premuto "Space", chiudi la finestra di dialogo
            CloseDialogueBox();

            // Imposta che l'utente ha risposto
            hasResponded = true;
        }
        else 
        {
            // Se non ci sono risposte, chiudi subito la finestra del dialogo
            CloseDialogueBox();
        }
    }

    private void CloseDialogueBox()
    {
        isOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;

        // Trigger the event when dialogue is closed
        OnDialogueClosed?.Invoke();
    }

    public void ResetResponseStatus() 
    {
        // Resetta la variabile hasResponded se vuoi permettere nuove interazioni
        hasResponded = false;
    }
}
