using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private Collider2D interactableObject;

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

    private void FixedUpdate()
    {
        Move();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            interactableObject = other;
            Debug.Log("Premi E per interagire con " + other.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == interactableObject)
        {
            interactableObject = null;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && interactableObject != null)
        {
            interactableObject.GetComponent<InteractableObject>()?.Interact();
        }
    }
}
