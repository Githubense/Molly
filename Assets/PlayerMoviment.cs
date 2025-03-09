using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUi dialogueUI;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private FixedJoystick joystick;

    public DialogueUi DialogueUi => dialogueUI;
    public IInteractable Interactable { get; set; }

    private bool hasInteracted = false;  // Track whether interaction was completed

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
        if (dialogueUI.isOpen) return;  // Prevent movement during dialogue

        Move(); // Handle movement

        if (Input.GetKeyDown(KeyCode.E) && !hasInteracted)
        {
            Interactable?.Interact(this);  // Trigger interaction with the object

            // If interaction triggers dialogue, wait for the response
            if (Interactable is DialogueActivator dialogueActivator)
            {
                hasInteracted = true; // Prevent further interaction until reset
            }
        }
    }

    private void Move()
    {
        Vector2 joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (joystickInput.sqrMagnitude > 0.01f)
        {
            moveInput = joystickInput;
        }
        else
        {
            moveInput = Vector2.zero;
        }

        rb.linearVelocity = moveInput * moveSpeed;

        // Update animator for walking direction
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

    // Method for resetting interaction when leaving area
    public void ResetInteraction()
    {
        hasInteracted = false;  // Reset interaction flag
        Interactable = null;    // Clear the interactable reference
    }
}
