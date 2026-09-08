using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class Dash3 : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float decayRate = 5f; // higher = faster slowdown

    private Rigidbody rb;
    private Vector3 lastMoveDirection = Vector3.forward;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Movement input (to capture last direction)
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        if (moveDir != Vector3.zero)
            lastMoveDirection = moveDir;
    }

    // Dash input (called by PlayerInput)
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing && dashCooldownTimer <= 0f)
        {
            StartCoroutine(Dash());
        }
    }

    private void Update()
    {
        // Cooldown timer
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        // Momentum decay after dash
        if (!isDashing && rb.linearVelocity.magnitude > 0.01f)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, decayRate * Time.deltaTime);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        rb.linearVelocity = lastMoveDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }
}

