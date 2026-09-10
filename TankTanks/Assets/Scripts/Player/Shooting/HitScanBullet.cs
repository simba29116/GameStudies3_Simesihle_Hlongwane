using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

public class HitScanBullet : MonoBehaviour
{ 

    private bool AddBulletSpread = true;
    public Vector3 BulletSpread = new Vector3(0.1f, 0.1f, 0.1f); // Adjust the spread values as needed
    public ParticleSystem MuzzleFlash; // Reference to the muzzle flash particle system 
    public Transform RayCastPoint; // Reference to the point from which the raycast will be fired
    public ParticleSystem ImpactEffect; // Reference to the impact effect particle system
    public TrailRenderer BulletTrail; // Reference to the bullet trail renderer
    public float ShootDelay = 0.5f; // Delay before the bullet is fired 
    private LayerMask Mask; // Layer mask to filter out unwanted layers during raycasting
    private float LastShootTime; // Time of the last shot fired



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void Shoot()
    {
       
        if (Time.time - LastShootTime < ShootDelay)
        {

            // Use Object Pooling for Muzzle Flash, Trial Renderer, and Impact Effect 
            MuzzleFlash.Play();
            Vector3 diretion = GetDirection();


            if (Physics.Raycast(RayCastPoint.position, diretion, out RaycastHit hit, float.MaxValue, Mask))
            {
                TrailRenderer trail = Instantiate(BulletTrail, RayCastPoint.position, Quaternion.identity);


                StartCoroutine(SpawnTrail(trail, hit.point));

                LastShootTime = Time.time;
            }
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
        Instantiate(ImpactEffect, hitPoint, Quaternion.LookRotation(hitPoint - startPosition));
        Destroy(trail.gameObject, trail.time);
    }



}


