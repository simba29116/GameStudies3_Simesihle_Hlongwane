using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    PoolManager poolManager;

    private void Awake()
    {
        poolManager= PoolManager.Instance;
    }

    void SpawnCube()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-10, 10), 0 ,Random.Range(-10, 10));
        poolManager.SpawnFromPool("cube", spawnPos, Quaternion.identity);
    }
}
