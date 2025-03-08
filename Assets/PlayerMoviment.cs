using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUi dialogueUI;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private FixedJoystick joystick;

    public DialogueUi DialogueUi => dialogueUI;
    public IInteractable Interactable { get; set; }

    // Make the HasInteracted property readable and writable
    public bool HasInteracted { get; set; }  // Now accessible and can be set

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Load saved position (if any)
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
        if (dialogueUI.isOpen) return; // Prevent movement during dialogue

        Move(); // Handle movement

        if (Input.GetKeyDown(KeyCode.E) && !HasInteracted) // Only interact if not already interacted
        {
            Interactable?.Interact(player: this);
            HasInteracted = true; // Mark as interacted
        }
    }

    private void Move()
    {
        Vector2 joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (joystickInput.sqrMagnitude > 0.01f) // Ignore minor joystick movements
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

    // Method for movement input (for Unity Input System)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Reset interaction state after a short time or when player leaves the area
    public void ResetInteraction()
    {
        HasInteracted = false;
    }
}
