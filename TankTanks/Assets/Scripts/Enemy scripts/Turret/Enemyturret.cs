using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public float detectionRange = 20f;

    [Header("Turret")]
    public Transform turretHead;
    public float rotationSpeed = 5f;

    [Header("Shooting")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate = 1f;

    [Header("Pooling")]
    public string bulletPoolTag = "EnemyBullet";

    private float nextFireTime;

    void Update()
    {
        if (target == null)
            return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= detectionRange)
        {
            RotateTowardsTarget();

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = target.position - turretHead.position;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        turretHead.rotation = Quaternion.Slerp(
        turretHead.rotation,
        targetRotation,
        rotationSpeed * Time.deltaTime
        );
    }

    void Shoot()
    {
        GameObject bullet = GenericPooling.Instance.SpawnFromPool(bulletPoolTag, firePoint.position, firePoint.rotation);
        if (bullet != null)
        {
            bullet.transform.SetPositionAndRotation(
            firePoint.position,
            firePoint.rotation
            );

            bullet.SetActive(true);
        }
    }
}
