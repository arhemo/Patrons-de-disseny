using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ProjectileFactory projectileFactory;
    [SerializeField] private NotificationManager notificationManager;

    private Rigidbody2D rb;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;

    [Header("Input")]
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction movementAction;
    [SerializeField] private InputAction shootAction;
    [SerializeField] private CoyoteJump coyoteJump;

    private bool jumpInputHeld;
    private bool jumpPressedThisFrame;

    [Header("Movement")]
    [SerializeField] private float horizontalMovementSpeed = 5f;

    private float horizontalVelocity;
    private float lastHorizontal = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        jumpAction.performed += OnJump;
        jumpAction.canceled += OnJump;
        movementAction.performed += OnMovement;
        movementAction.canceled += OnMovement;
        shootAction.performed += OnShoot;
    }

    private void OnEnable()
    {
        jumpAction.Enable();
        movementAction.Enable();
        shootAction.Enable();
    }

    private void OnDisable()
    {
        jumpAction.Disable();
        movementAction.Disable();
        shootAction.Disable();

        jumpInputHeld = false;
        jumpPressedThisFrame = false;
    }

    private void OnDestroy()
    {
        jumpAction.performed -= OnJump;
        jumpAction.canceled -= OnJump;
        movementAction.performed -= OnMovement;
        movementAction.canceled -= OnMovement;
        shootAction.performed -= OnShoot;
    }

    private void Update()
    {
        if (rb == null || coyoteJump == null)
            return;

        isGrounded = groundCheck != null && Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        coyoteJump.UpdateJump(rb, isGrounded, jumpPressedThisFrame, jumpInputHeld);

        jumpPressedThisFrame = false;

        rb.linearVelocity = new Vector2(horizontalVelocity, rb.linearVelocity.y);
    }

    private void FixedUpdate()
    {
        if (rb == null || coyoteJump == null)
            return;

        coyoteJump.FixedUpdateJump(rb, isGrounded, jumpInputHeld);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        bool isPressed = context.ReadValue<float>() > 0.5f;

        if (isPressed && !jumpInputHeld)
            jumpPressedThisFrame = true;

        jumpInputHeld = isPressed;
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();

        horizontalVelocity = input * horizontalMovementSpeed;

        if (Mathf.Abs(input) > 0f)
            lastHorizontal = Mathf.Sign(input);
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0.5f)
            Shoot();
    }

    private void Shoot()
    {
        if (projectileFactory == null)
            return;

        float direction = lastHorizontal;

        projectileFactory.Create(transform.position + Vector3.right * direction, new Vector2(direction, 0f));
    }

    public void PickUpCoin()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.AddCoins(1);

        if (notificationManager == null)
            return;

        int totalCoins = GameManager.Instance.Coins;

        notificationManager.AddNotification("Player now has " + totalCoins + (totalCoins == 1 ? " coin" : " coins"), 1.5f);
    }
}