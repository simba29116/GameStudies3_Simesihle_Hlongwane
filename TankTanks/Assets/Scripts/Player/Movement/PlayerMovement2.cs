using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour
{
    public Rigidbody rb;
    private Vector2 moveInput;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float isoAngle = 45f;
    public float rotationSpeed = 10f;

    public Transform bodyChild;



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
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = Quaternion.Euler(0, isoAngle, 0) * move;

        rb.linearVelocity = move * moveSpeed;

        if (move.sqrMagnitude > 0.01f && bodyChild != null)
        {
            Quaternion targetRot = Quaternion.LookRotation(move, Vector3.up);
            bodyChild.rotation = Quaternion.Slerp(bodyChild.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
