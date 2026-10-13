using UnityEngine;

[System.Serializable]
public class CoyoteJump
{
    public float jumpForce = 14f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    public void FixedUpdateJump(Rigidbody2D body, bool isGrounded, bool jumpInputHeld)
    {
        if (body == null)
            return;

        if (body.linearVelocity.y < 0f)
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        else if (body.linearVelocity.y > 0f && !jumpInputHeld)
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
    }

    public void UpdateJump(Rigidbody2D body, bool isGrounded, bool jumpPressedThisFrame, bool jumpInputHeld)
    {
        if (body == null)
            return;

        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (jumpPressedThisFrame)
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;

            GameEvents.PlayerJumped();
        }

        if (!jumpInputHeld && body.linearVelocity.y > 0f)
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y * 0.5f);
    }
}