using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static PoolManager Instance; 


    [System.Serializable]
    public class Pool
    {
        public int size;
        public GameObject prefab;
        public string tag;
    }

    [SerializeField] 
    public  List<Pool> pools=new List<Pool>();

    public Dictionary<string, Queue<GameObject>> poolDictionary; 
    private void Awake()
    {
        
            Instance= this;
        
    }



    private void Start()
    {

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(var pool in pools)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject temp = Instantiate(pool.prefab);
                temp.SetActive(false);
                poolQueue.Enqueue(temp);
            }

            poolDictionary.Add(pool.tag, poolQueue);

        }
    }




    public GameObject SpawnFromPool(string tag, Vector2 pos, Quaternion rot)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Tag Doesn't exist");
            return null;
        }

        GameObject toSpawn= poolDictionary[tag].Dequeue();

        toSpawn.transform.position = pos;
        toSpawn.transform.rotation = rot;
        toSpawn.SetActive(true);

        poolDictionary[tag].Enqueue(toSpawn);

        return toSpawn;
    }


}
