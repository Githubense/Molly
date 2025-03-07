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

    private List<Button> tempResponseButton = new List<Button>(); // Cambiato per memorizzare solo i Button
    private bool hasResponded = false; // Variabile di stato per verificare se è stata già selezionata una risposta

    private void Start()
    {
        dialogueUI = GetComponent<DialogueUi>();
    }

    public void ShowResponses(Response[] responses)
    {
        if (hasResponded) return; // Se una risposta è già stata selezionata, non fare nulla

        float responseBoxHeight = 0;

        foreach (Response response in responses)
        {
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            Button buttonComponent = responseButton.GetComponent<Button>();

            // Aggiungi l'ascoltatore dell'evento di clic solo se non è stata data una risposta
            buttonComponent.onClick.AddListener(() => OnPickedResponse(response));

            // Aggiungi il pulsante alla lista
            tempResponseButton.Add(buttonComponent);

            // Calcola l'altezza del box di risposta
            responseBoxHeight += responseButtonTemplate.sizeDelta.y;
        }

        // Imposta l'altezza dinamica del box
        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);

        // Rendi visibile la risposta
        responseBox.gameObject.SetActive(true);
    }

    private void OnPickedResponse(Response response)
    {
        if (hasResponded) return; // Se la risposta è già stata scelta, non fare nulla

        hasResponded = true; // Imposta che una risposta è stata scelta

        // Nascondi la risposta dopo la selezione
        responseBox.gameObject.SetActive(false);

        // Fai in modo che i pulsanti non reagiscano più
        foreach (Button button in tempResponseButton)
        {
            button.onClick.RemoveAllListeners(); // Rimuovi tutti gli ascoltatori di eventi
        }

        // Resetta la lista dei pulsanti (se non la stai più usando)
        tempResponseButton.Clear();

        // Mostra il dialogo associato alla risposta
        dialogueUI.ShowDialogue(response.DialogueObject);
    }
}
