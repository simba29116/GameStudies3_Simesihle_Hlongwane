using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class HitScanBullet2 : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem MuzzleFlash;
    public Transform RayCastPoint;
    public ParticleSystem ImpactEffect;
    public TrailRenderer BulletTrail;

    [Header("Settings")]
    public bool AddBulletSpread = true;
    public Vector3 BulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    public float ShootDelay = 0.2f; // shorter delay for automatic fire
    public LayerMask Mask;

    private float LastShootTime;
    private bool isShooting;

    // Input System
    private PlayerInput playerInput;
    private InputAction shootAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Shooting"]; //ShootAction
    }

    void OnEnable()
    {
        shootAction.performed += Shooting;
        shootAction.canceled += OnShootCanceled;
    }

    void OnDisable()
    {
        shootAction.performed -= Shooting
            ;
        shootAction.canceled -= OnShootCanceled;
    }

    private void Shooting(InputAction.CallbackContext ctx)
    {
        isShooting = true;
    }

    private void OnShootCanceled(InputAction.CallbackContext ctx)
    {
        isShooting = false;
    }

    void Update()
    {
        if (isShooting && Time.time - LastShootTime >= ShootDelay)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        LastShootTime = Time.time;

        if (MuzzleFlash != null)
            MuzzleFlash.Play();

        Vector3 direction = GetDirection();

        Debug.DrawRay(RayCastPoint.position, direction * 100f, Color.red, 1f);

        if (Physics.Raycast(RayCastPoint.position, direction, out RaycastHit hit, Mathf.Infinity, Mask))
        {
            TrailRenderer trail = Instantiate(BulletTrail, RayCastPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit.point));

            if (ImpactEffect != null)
                Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = RayCastPoint.forward;
        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpread.x, BulletSpread.x),
                Random.Range(-BulletSpread.y, BulletSpread.y),
                Random.Range(-BulletSpread.z, BulletSpread.z)
            );
        }
        return direction.normalized;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = hitPoint;
        Destroy(trail.gameObject, trail.time);
    }
}


