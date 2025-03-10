using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUi dialogueUI;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private FixedJoystick joystick;

    public DialogueUi DialogueUi => dialogueUI;
    public IInteractable Interactable { get; set; }  // Assigned by DialogueActivator

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
        // If dialogue is open, disable movement so the player doesn't walk around
        if (dialogueUI.isOpen) return;

        Move();

        // Press E to interact with the currently assigned Interactable
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactable?.Interact(this);
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
}
