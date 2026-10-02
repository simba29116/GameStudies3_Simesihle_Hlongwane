using System.Collections.Generic;
using UnityEngine;

public class GenericPooling : MonoBehaviour
{
    public static GenericPooling Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;

    private Dictionary<string, List<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;

        poolDictionary = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools)
        {
            List<GameObject> objects = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);

                objects.Add(obj);
            }

            poolDictionary.Add(pool.tag, objects);
        }
    }

    public GameObject SpawnFromPool( string tag, Vector3 position,Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool '{tag}' not found.");
            return null;
        }

        foreach (GameObject obj in poolDictionary[tag])
        {
            if (obj == null)
                continue;

            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);

                return obj;
            }
        }

        Debug.LogWarning($"Pool '{tag}' exhausted.");
        return null;
    }


    
}



