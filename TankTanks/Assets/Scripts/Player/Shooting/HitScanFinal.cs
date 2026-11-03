using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;


public class HitScanFinal : MonoBehaviour
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

    [Header("Heat")]
    public float MaxHeat = 100f;
    public float CurrentHeat = 0f;

    public float HeatPerShot = 10f;
    public float CoolRate = 15f;
    public float OverheatThreshold = 100f;
    public float CooldownThreshold = 30f;
    private bool isOverheated;

  

    [Header("UI")]
    public Image HeatBar;
    public Image OverheatWarning; 
    



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
        
        UpdateHeatUI();
        showimage();
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

        if (CurrentHeat > 0)
        {
            CurrentHeat -= CoolRate * Time.deltaTime;
            CurrentHeat = Mathf.Clamp(CurrentHeat, 0, MaxHeat);

            UpdateHeatUI();

            if (isOverheated && CurrentHeat <= CooldownThreshold)
            {
                isOverheated = false;
            }


        }

        if (isShooting &&
        Time.time - LastShootTime >= ShootDelay &&!isOverheated)
        {
            Shoot();
        }

    }

    public void Shoot()
    {
        CurrentHeat += HeatPerShot;

        if (CurrentHeat >= OverheatThreshold)
        {
            CurrentHeat = MaxHeat;
            isOverheated = true;
        }
        
        UpdateHeatUI();

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

    private void UpdateHeatUI()
    {
        if (HeatBar != null)
        {
            HeatBar.fillAmount = CurrentHeat / MaxHeat;
        }

        float heatPercent = CurrentHeat / MaxHeat;
        if (OverheatWarning != null)
        {
            Color warningColor = OverheatWarning.color;

            // Starts becoming visible at 60% heat
           float warningAlpha =
                Mathf.InverseLerp(0.6f, 1f, heatPercent);

            warningColor.a = warningAlpha * 0.4f;

            OverheatWarning.color = warningColor;
        }
    }

    public void showimage()
    {
        OverheatWarning.enabled = true;
    }

}

