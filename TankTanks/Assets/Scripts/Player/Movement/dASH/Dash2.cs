using UnityEngine;
using UnityEngine.InputSystem;


public class Dash2 : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool canDash = true;

    [Header("Dash Settings")]
    public float dashForce = 15f;       // strength of impulse
    public float dashCooldown = 0.7f;   // cooldown between dashes
    public float dashDuration = 0.2f;   // optional: how long dash effect lasts

    private bool isDashing;
    private Vector3 dashDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Movement input (needed for direction)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Dash input
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && moveInput.sqrMagnitude > 0.01f)
        {
            StartDash();
        }
    }

    private void StartDash()
    {
        // Camera-relative direction
        Camera cam = Camera.main;
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

        dashDirection = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        // Apply impulse force
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        // State + cooldown
        isDashing = true;
        canDash = false;

        Invoke(nameof(EndDash), dashDuration);
        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void EndDash()
    {
        isDashing = false;
    }

    private void ResetDash()
    {
        canDash = true;
    }
}
