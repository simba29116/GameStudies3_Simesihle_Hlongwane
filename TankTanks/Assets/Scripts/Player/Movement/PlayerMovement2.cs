using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour
{
    public Rigidbody rb;
    private Vector2 moveInput;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float isoAngle = 45f; // Adjust to match your isometric camera angle

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent unwanted rotation
    }

    // Input System callback for Move action
    public void MovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Convert 2D input into 3D isometric movement
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Rotate input to align with isometric camera angle
        move = Quaternion.Euler(0, isoAngle, 0) * move;

        // Apply velocity directly
        rb.linearVelocity = move * moveSpeed;
    }
}
