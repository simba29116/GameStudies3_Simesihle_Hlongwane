using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement3 : MonoBehaviour
{

    private Rigidbody rb;
    private Vector2 moveInput;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

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

    private void FixedUpdate()
    {
        
        Vector3 camForward = mainCam.transform.forward;
        Vector3 camRight = mainCam.transform.right;

        
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        
        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;

        rb.linearVelocity = move * moveSpeed;

       
        if (move.sqrMagnitude > 0.01f && bodyChild != null)
        {
            Quaternion targetRot = Quaternion.LookRotation(move, Vector3.up);
            bodyChild.rotation = Quaternion.Slerp(bodyChild.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}


