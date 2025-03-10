using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUi dialogueUI;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private FixedJoystick joystick;

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
        // If a dialogue is open, we skip movement (so the player won't move while reading).
        // But we allow continuing/closing the dialogue via SPACE in DialogueUI itself.
        if (!dialogueUI.isOpen)
        {
            Move();

            // Press E to interact, no longer blocked by any hasInteracted flag
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interactable?.Interact(this);
            }
        }
    }

    private void Move()
    {
        // Gather input from the virtual joystick
        Vector2 joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (joystickInput.sqrMagnitude > 0.01f)
            moveInput = joystickInput;
        else
            moveInput = Vector2.zero;

        // Set movement
        rb.linearVelocity = moveInput * moveSpeed;

        // Update animator for walking direction
        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);

            // Store last input direction so idle animation “faces” that way
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
