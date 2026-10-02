using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class HitScanBullet4 : MonoBehaviour
{
    [Header("References")]
    public Transform RayCastPoint;

    [Header("Pool Tags")]
    public string TrailPoolTag = "BulletTrail";
    public string ImpactPoolTag = "ImpactEffect";
    public string MuzzleFlashPoolTag = "MuzzleFlash";

    [Header("Settings")]
    public bool AddBulletSpread = true;
    public Vector3 BulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    public float ShootDelay = 0.2f;
    public float MissDistance = 200f;
    public LayerMask Mask;

    [Header("Damage")]
    public float Damage = 10f;

    [Header("Ammo")]
    public int MaxAmmo = 10;
    public int CurrentAmmo;
    public float ReloadTime = 2f;

    private bool isReloading;

    [Header("UI")]
    public TMP_Text AmmoText;


    private float LastShootTime;
    private bool isShooting;

    private PlayerInput playerInput;
    private InputAction shootAction;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Shooting"];
    }

    private void Start()
    {
        CurrentAmmo = MaxAmmo;
        UpdateAmmoUI();
    }
    
    private void OnEnable()
    {
        shootAction.performed += Shooting;
        shootAction.canceled += OnShootCanceled;
    }

    private void OnDisable()
    {
        shootAction.performed -= Shooting;
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

    private void Update()
    {

        if (isReloading)
            return;

        if (isShooting &&Time.time - LastShootTime >= ShootDelay && CurrentAmmo > 0)
        {
            Shoot();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame &&
        CurrentAmmo < MaxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    public void Shoot()
    {
        if (CurrentAmmo <= 0)
            return;

        CurrentAmmo--;
        UpdateAmmoUI();

        LastShootTime = Time.time;

        // Muzzle Flash
        SpawnMuzzleFlash();

        Vector3 direction = GetDirection();

        Debug.DrawRay(RayCastPoint.position, direction * MissDistance, Color.red, 1f);

        Vector3 endPoint;

        if (Physics.Raycast(
            RayCastPoint.position, direction, out RaycastHit hit, Mathf.Infinity, Mask))
        {
            endPoint = hit.point;

            SpawnImpact(hit);
            //Damage Enemy 
            EnemyHealth enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Damage);
                Debug.Log($"Hit {hit.collider.name} for {Damage} damage.");
            }
            
        }
        else
        {
            endPoint = RayCastPoint.position + direction * MissDistance;
        }

        SpawnBulletTrail(endPoint);


        if (CurrentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void SpawnBulletTrail(Vector3 endPoint)
    {
        GameObject trailObj = GenericPooling.Instance.SpawnFromPool(TrailPoolTag, RayCastPoint.position, Quaternion.identity);

        if (trailObj == null)
            return;

        TrailRenderer trail = trailObj.GetComponent<TrailRenderer>();

        trail.Clear();

        StartCoroutine(AnimateTrail(trail, endPoint));
    }

    private void SpawnImpact(RaycastHit hit)
    {
        GameObject impactObj = GenericPooling.Instance.SpawnFromPool(ImpactPoolTag, hit.point, Quaternion.LookRotation(hit.normal));

        if (impactObj == null)
            return;

        ParticleSystem impact =
            impactObj.GetComponent<ParticleSystem>();

        impact.Clear();
        impact.Play();

        StartCoroutine(
            DisableParticleAfterDuration(
                impact));
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = RayCastPoint.forward;

        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpread.x, BulletSpread.x),
                Random.Range(-BulletSpread.y, BulletSpread.y),
                Random.Range(-BulletSpread.z, BulletSpread.z));
        }

        return direction.normalized;
    }

    private IEnumerator AnimateTrail(
        TrailRenderer trail,
        Vector3 hitPoint)
    {
        float time = 0f;
        Vector3 startPosition = trail.transform.position;

        while (time < 1f)
        {
            trail.transform.position =
                Vector3.Lerp(
                    startPosition,
                    hitPoint,
                    time);

            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hitPoint;

        yield return new WaitForSeconds(trail.time);

        trail.Clear();
        trail.gameObject.SetActive(false);
    }

    private IEnumerator DisableParticleAfterDuration(ParticleSystem particle)
    {
        if (particle == null)
            yield break;

        while (particle != null && particle.IsAlive(true))
        {
            yield return null;
        }

        if (particle == null)
            yield break;

        particle.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        particle.gameObject.SetActive(false);
    }


    private void SpawnMuzzleFlash()
    {
        GameObject muzzleObj = GenericPooling.Instance.SpawnFromPool(
            MuzzleFlashPoolTag,
            RayCastPoint.position,
            RayCastPoint.rotation
        );

        if (muzzleObj == null)
            return;

        ParticleSystem muzzle = muzzleObj.GetComponent<ParticleSystem>();

        if (muzzle == null)
        {
            Debug.LogWarning(
                $"Muzzle flash object '{muzzleObj.name}' has no ParticleSystem."
            );
            return;
        }

        muzzle.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        muzzle.Play();

        StartCoroutine(DisableParticleAfterDuration(muzzle));
    }

    private void UpdateAmmoUI()
    {
        if (AmmoText != null)
        {
            AmmoText.text = $"{CurrentAmmo}/{MaxAmmo}";
        }
    }

    private IEnumerator Reload()
    {
        if (isReloading)
            yield break;

        isReloading = true;

        yield return new WaitForSeconds(ReloadTime);

        CurrentAmmo = MaxAmmo;

        UpdateAmmoUI();

        isReloading = false;

       
    }

}

