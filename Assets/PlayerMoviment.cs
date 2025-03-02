using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUi dialogueUI;
    [SerializeField] private float moveSpeed = 5f;
    public DialogueUi DialogueUi => dialogueUI;
    public IInteractable Interactable { get; set; }
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Controlla se esiste una posizione salvata
        Vector3 savedPosition = PlayerPositionManager.Instance.GetSavedPosition(SceneManager.GetActiveScene().buildIndex);

        if (savedPosition != Vector3.zero)
        {
            transform.position = savedPosition;
            Debug.Log("Caricata posizione: " + savedPosition + " nella scena " + SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log("Nessuna posizione salvata per la scena " + SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Update()
    {
        if (dialogueUI.isOpen) return;

         Move();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactable?.Interact(player:this);
            
        }
    }

    private void Move()
    {
        rb.linearVelocity = moveInput * moveSpeed;

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
