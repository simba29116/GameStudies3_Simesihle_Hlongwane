using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EnemyShooter : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 8f;
    public LayerMask playerLayer;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform[] firePoints; // MUST BE SIZE 2 IN INSPECTOR
    public float fireRate = 1f;
    public float bulletSpeed = 20f;

    private Rigidbody rb;
    private Transform player;
    private bool playerInRange = false;
    private Coroutine shootingRoutine;

    private int currentFireIndex = 0; // 0 ? first fire point, 1 ? second fire point

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        DetectPlayer();

        if (playerInRange)
            TrackPlayer();
    }

    private void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        if (hits.Length > 0)
        {
            if (!playerInRange)
            {
                playerInRange = true;
                player = hits[0].transform;
                shootingRoutine = StartCoroutine(ShootRoutine());
            }
        }
        else
        {
            if (playerInRange)
            {
                playerInRange = false;
                player = null;

                if (shootingRoutine != null)
                    StopCoroutine(shootingRoutine);
            }
        }
    }

    private void TrackPlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position);
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 8f * Time.deltaTime);
        }
    }

    private IEnumerator ShootRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(1f / fireRate);

        while (true)
        {
            Shoot();
            yield return delay;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoints.Length < 2 || player == null)
            return;

        Transform fp = firePoints[currentFireIndex];

        // Calculate direction from fire point ? player
        Vector3 targetPos = player.position;
        
        Vector3 direction = (targetPos - fp.position).normalized;

        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, fp.position, Quaternion.LookRotation(direction));

        // Apply velocity directly toward player
        if (bullet.TryGetComponent<Rigidbody>(out Rigidbody rbBullet))
            rbBullet.linearVelocity = direction * bulletSpeed;

        // Alternate fire points
        currentFireIndex = (currentFireIndex + 1) % firePoints.Length;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}