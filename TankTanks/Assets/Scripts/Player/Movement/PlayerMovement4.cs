using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement4 : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 lastMoveDirection;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    [Header("References")]
    public Transform bodyChild;
    public Camera mainCam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (mainCam == null)
            mainCam = Camera.main;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing && dashCooldownTimer <= 0f)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return; // skip normal movement while dashing

        Vector3 camForward = mainCam.transform.forward;
        Vector3 camRight = mainCam.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;

        rb.linearVelocity = move * moveSpeed;

        if (move.sqrMagnitude > 0.01f)
            lastMoveDirection = move; // store last direction

        if (move.sqrMagnitude > 0.01f && bodyChild != null)
        {
            Quaternion targetRot = Quaternion.LookRotation(move, Vector3.up);
            bodyChild.rotation = Quaternion.Slerp(bodyChild.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }

        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.fixedDeltaTime;
    }

    private IEnumerator Dash()
    {
        isDashing = true;

        // Use last move direction if available, otherwise dash forward
        Vector3 dashDir = lastMoveDirection != Vector3.zero ? lastMoveDirection : transform.forward;

        // Apply impulse force instead of setting velocity
        rb.AddForce(dashDir.normalized * dashSpeed, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }

}

