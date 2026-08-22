using UnityEngine;
using UnityEngine.InputSystem;
public class AimingBehaviour : MonoBehaviour
{

    private Vector2 Aimming;
    
    //Aim settings 
    public float isoAngle = 45f;
    public float speedRotation = 10f;

    //Child References 
    public Transform weaponChild;


    public void AimInput(InputAction.CallbackContext context)
    {
        Aimming = context.ReadValue<Vector2>();
    }

    
    void Update()
    {
       if(Aimming.sqrMagnitude > 0.01f && weaponChild != null)
        {
            Vector3 aimDirection = new Vector3(Aimming.x, 0, Aimming.y);
            aimDirection = Quaternion.Euler(0, isoAngle, 0) * aimDirection;
            Quaternion targetRot = Quaternion.LookRotation(aimDirection, Vector3.up);
            weaponChild.rotation = Quaternion.Slerp(weaponChild.rotation, targetRot, speedRotation * Time.deltaTime);
        }


    }
}
