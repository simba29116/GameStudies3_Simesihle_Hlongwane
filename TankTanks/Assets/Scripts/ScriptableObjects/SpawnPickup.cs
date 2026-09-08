using UnityEngine;

public class SpawnPickup : MonoBehaviour
{

    public PickUpItem[] pickup;

    public GameObject emptyPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            GameObject temp= Instantiate(emptyPrefab, this.transform);
            int num= Random.Range(0, pickup.Length);
            
            temp.GetComponent<MeshFilter>().mesh = pickup[num].mesh;
            temp.GetComponent<MeshRenderer>().material = pickup[num].material;



        }

    }
}
