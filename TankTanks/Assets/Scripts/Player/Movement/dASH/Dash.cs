using UnityEngine;
using UnityEngine.InputSystem;


public class Dash : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool canDash = true;
    private float lastDashTime;

    [Header("Dash Settings")]
    public float dashForce = 10f;
    public float dashCooldown = 0.7f;
    public float dashDuration = 0.2f; // how long dash lasts

    private Vector3 dashDirection;
    private bool isDashing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && moveInput.sqrMagnitude > 0.01f)
        {
            StartDash();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashForce;
        }
    }

    private void StartDash()
    {
        
        dashDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        
        Camera cam = Camera.main;
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0; camRight.y = 0;
        dashDirection = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        // Begin dash
        isDashing = true;
        canDash = false;
        lastDashTime = Time.time;

        // End dash after duration
        Invoke(nameof(EndDash), dashDuration);

        // Reset cooldown
        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void EndDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector3.zero; // stop dash movement
    }

    private void ResetDash()
    {
        canDash = true;
    }
}

