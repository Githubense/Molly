using UnityEngine;
using System.Collections;

public class ShowImageBeforeDialogue : MonoBehaviour, IInteractable
{
    [Header("Riferimento al DialogueActivator esistente (stesso GameObject o un altro)")]
    [SerializeField] private DialogueActivator dialogueActivator;

    [Header("Immagine da mostrare in primo piano")]
    [SerializeField] private GameObject imageToShow;

    [Header("Chiave di interazione (opzionale, se vuoi legare logica a StorylineManager)")]
    [SerializeField] private string interactionKey;

    private bool canInteract = true;

    private void Awake()
    {
        // Assicuriamoci che l'immagine sia nascosta all'inizio
        if (imageToShow != null)
            imageToShow.SetActive(false);

        // (Opzionale) Se non vuoi che DialogueActivator gestisca le collisioni,
        // potresti disabilitare il suo collider o il suo script, es:
        // var col = dialogueActivator.GetComponent<Collider2D>();
        // if (col) col.enabled = false;
        // oppure dialogueActivator.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se il Player entra e posso interagire, mi imposto come Interactable
        if (other.CompareTag("Player") && canInteract)
        {
            if (other.TryGetComponent<PlayerMovement>(out PlayerMovement player))
            {
                player.Interactable = this;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Se il Player esce, tolgo l'interactable
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerMovement>(out PlayerMovement player) &&
                player.Interactable == this)
            {
                player.Interactable = null;
            }
        }
    }

    public void Interact(PlayerMovement player)
    {
        // Quando il player preme E su di me:
        if (!canInteract) return;

        // 1) Mostra l’immagine
        if (imageToShow != null)
            imageToShow.SetActive(true);

        canInteract = false;

        // 2) Avvia una Coroutine che aspetta un attimo (o un input) e poi
        //    chiama il DialogueActivator reale.
        StartCoroutine(ShowImageAndThenDialogue(player));
    }

  [SerializeField] private float showImageDuration = 3f; // Modificabile da Inspector

private IEnumerator ShowImageAndThenDialogue(PlayerMovement player)
{
    // L'immagine diventa visibile:
    if (imageToShow != null)
        imageToShow.SetActive(true);

    // Qui aspetti 'showImageDuration' secondi
    yield return new WaitForSeconds(showImageDuration);

    // Nascondi l’immagine (se vuoi)
    if (imageToShow != null)
        imageToShow.SetActive(false);

    // Ora chiami il dialogo
    if (dialogueActivator != null)
    {
        dialogueActivator.Interact(player);
    }
}

}
