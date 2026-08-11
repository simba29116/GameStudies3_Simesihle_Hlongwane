using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private Vector2 movement; 
    public Rigidbody rb;
    public float moveSpeed;

    public void MovementInput(InputAction.CallbackContext context)
    {
        movement= context.ReadValue<Vector2>();
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        if (context.performed)
        {
            Jump();
        }
    }



    public void Jump()
    {
        rb.AddForce(15 * transform.up );
    }
    private void Move()
    {
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.y)*moveSpeed;

    }

    private void Update()
    {
        Move(); 
    }
}
