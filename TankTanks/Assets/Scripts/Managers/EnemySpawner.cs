using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] abilityPrefabs;

    [SerializeField] private int minEnemies = 4;
    [SerializeField] private int maxEnemies = 10;

    [SerializeField] private int minAbilities = 1;
    [SerializeField] private int maxAbilities = 3;

    [SerializeField] private Transform[] spawnPoints;

    [System.Serializable]
    public class EnemyEntry
    {
        public GameObject prefab;
        public int weight;
    }
    
    [SerializeField] private EnemyEntry[] enemies;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   /* GameObject GetRandomEnemy()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }*/

    GameObject GetRandomAbility()
    {
        return abilityPrefabs[Random.Range(0, abilityPrefabs.Length)];
    }


    void SpawnWave()
    {
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        // Guaranteed enemies
        for (int i = 0; i < minEnemies; i++)
        {
            SpawnObject(GetWeightedEnemy(), availablePoints);
        }

        // Guaranteed abilities
        for (int i = 0; i < minAbilities; i++)
        {
            SpawnObject(GetRandomAbility(), availablePoints);
        }

        // Extra random enemies
        for (int i = minEnemies; i < maxEnemies; i++)
        {
            if (Random.value < 0.5f)
            {
                SpawnObject(GetWeightedEnemy(), availablePoints);
            }
        }

        // Extra random abilities
        for (int i = minAbilities; i < maxAbilities; i++)
        {
            if (Random.value < 0.25f)
            {
                SpawnObject(GetRandomAbility(), availablePoints);
            }
        }
    }


    void SpawnObject(GameObject prefab, List<Transform> availablePoints)
    {
        if (availablePoints.Count == 0)
            return;

        int randomIndex = Random.Range(0, availablePoints.Count);
        Transform spawnPoint = availablePoints[randomIndex];

        Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        availablePoints.RemoveAt(randomIndex);
    }


    GameObject GetWeightedEnemy()
    {
        int totalWeight = 0;

        foreach (EnemyEntry enemy in enemies)
            totalWeight += enemy.weight;

        int random = Random.Range(0, totalWeight);

        foreach (EnemyEntry enemy in enemies)
        {
            if (random < enemy.weight)
                return enemy.prefab;

            random -= enemy.weight;
        }

        return enemies[0].prefab;
    }
}
