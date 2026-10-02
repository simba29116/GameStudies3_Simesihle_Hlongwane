using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject shotPrefab;
    public Transform[] gunPoints;
    public float fireRate;

    bool firing;
    float fireTimer;

    int gunPointIndex;

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= 1 / fireRate)
        {
            Fire(); // trigger firing
            fireTimer = 0f;
        }

        if (firing)
        {
            SpawnShot();
            firing = false;
        }
    }

    void SpawnShot()
    {
        var gunPoint = gunPoints[gunPointIndex++];
        Instantiate(shotPrefab, gunPoint.position, gunPoint.rotation);
        gunPointIndex %= gunPoints.Length;



        
    }

    public void Fire()
    {
        firing = true;
    }
}
