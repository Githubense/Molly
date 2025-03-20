using UnityEngine;
using UnityEngine.InputSystem;

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
    }

    private void Update()
    {
        if (dialogueUI.isOpen) return;

        Move();

        if (Keyboard.current.eKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame || Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            Interactable?.Interact(this);
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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Interactable?.Interact(this);
        }
    }
}