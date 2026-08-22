using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming2 : MonoBehaviour
{

    private Camera mainCamera;
    private Vector2 mouseScreenPosition;


    //Aim settings
    public float rotationSpeed = 10f;
    public Transform weaponChild; // toward Crosshair

    private void Awake()
    {
        mainCamera = Camera.main;
    }


    public void Aimming(InputAction.CallbackContext context)
    {
        mouseScreenPosition = context.ReadValue<Vector2>();
    }


    void Update()
    {

        if (weaponChild == null || mainCamera == null) return;

        //raycast for the mouse position
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);

        //Define Ray at ground point y = 0
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);



        if (groundPlane.Raycast(ray, out float enter))
        {
            // Get world position of mouse on ground plane
            Vector3 hitPoint = ray.GetPoint(enter);

            // Direction from weapon to hit point
            Vector3 aimDir = (hitPoint - weaponChild.position).normalized;
            aimDir.y = 0; // keep rotation flat on ground plane

            // Rotate weapon toward aim direction
            if (aimDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(aimDir, Vector3.up);
                weaponChild.rotation = Quaternion.Slerp(
                    weaponChild.rotation,
                    targetRot,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}

