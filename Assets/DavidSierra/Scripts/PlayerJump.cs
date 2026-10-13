using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] NotificationManager notificationManager;
    private Rigidbody2D rb;
    int coins = 0;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;


    private bool isGrounded;

    [Header("Input")]
    [SerializeField] InputAction jumpAction;
    [SerializeField] InputAction movementAction;
    [SerializeField] InputAction shootAction;

    [SerializeField] CoyoteJump coyoteJump;
    bool jumpInput = false;

    [Header("Movement")]
    [SerializeField] float horizontalMovementSpeed = 5.0f;
    float horizontalVelocity;
    float lastHorizontal = 1.0f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        jumpAction.performed += OnJump;
        movementAction.performed += OnMovement;
        shootAction.performed += OnShoot;
        lastHorizontal = 1.0f;
    }
    private void OnEnable() {
        jumpAction.Enable();
        movementAction.Enable();
        shootAction.Enable();
    }
    private void OnDisable() {
        jumpAction.Disable();
        movementAction.Disable();
        shootAction.Disable();
    }

    void Update() {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        coyoteJump.UpdateJump(rb, isGrounded, jumpInput);
        rb.linearVelocity = new Vector2(horizontalVelocity, rb.linearVelocity.y);
    }

    void FixedUpdate() {
        coyoteJump.FixedUpdateJump(rb, isGrounded, jumpInput);
    }
    void OnJump(InputAction.CallbackContext ctx) {
        if (ctx.ReadValue<float>() == 1) {
            jumpInput = true;
        } else {
            jumpInput = false;
        }
    }

    void OnMovement(InputAction.CallbackContext ctx) {
        horizontalVelocity = ctx.ReadValue<float>() * horizontalMovementSpeed;
        if (Mathf.Abs(ctx.ReadValue<float>()) > 0) lastHorizontal = horizontalVelocity;
    }
    void OnShoot(InputAction.CallbackContext ctx) {
        if (ctx.ReadValue<float>() == 1.0f) {
            Shoot();
        }
    }

    void Shoot() {
        GameObject projectileInstance = Instantiate(projectilePrefab);
        Projectile projectile = projectileInstance.GetComponent<Projectile>();
        projectile.transform.position = transform.position + (Vector3.right * Mathf.Clamp(lastHorizontal, -1, 1));
        projectile.Init(new Vector2(lastHorizontal, 0));

    }

    public void PickUpCoin() {
        coins++;
        notificationManager.AddNotification("Player now has " + coins + " coin" + (coins > 1 ? "s" : ""), 1.5f);
    }


}

[System.Serializable]
public class CoyoteJump {
    public float jumpForce = 14f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float coyoteTime = 0.1f; // Time after leaving ground where jump is still allowed
    public float jumpBufferTime = 0.1f; // Buffer time for jump input
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    public void FixedUpdateJump(Rigidbody2D rigidbody2D, bool isGrounded, bool jumpInput) {
        // Better gravity
        if (rigidbody2D.linearVelocity.y < 0) {
            rigidbody2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        } else if (rigidbody2D.linearVelocity.y > 0 && !jumpInput) {
            rigidbody2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    public void UpdateJump(Rigidbody2D rigidbody2D, bool isGrounded, bool jumpInputPressed) {

        // Update coyote timer
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Jump buffer
        if (jumpInputPressed)
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // Jump
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f) {
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f; // consume buffer
        }

        // Variable jump height
        if (!jumpInputPressed && rigidbody2D.linearVelocity.y > 0f) {
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, rigidbody2D.linearVelocity.y * 0.5f);
            coyoteTimeCounter = 0f; // prevent double use
        }
    }

}
