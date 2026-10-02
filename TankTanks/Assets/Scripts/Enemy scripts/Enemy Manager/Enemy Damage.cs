
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private float hitpoints;
    public float Maxhitpoints = 7f;

    
    [SerializeField] private float cardDamage;
    [SerializeField] private float dwayneDamage;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitpoints = Maxhitpoints;
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        hitpoints -= damage;
        if (hitpoints <= 0)
        {



            Destroy(gameObject);
            
               
          
            
            

        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Card"))
        {
            TakeDamage(cardDamage);
        }

        if (collision.gameObject.CompareTag("Dwayne"))
        {
            TakeDamage(dwayneDamage);
        }



    }

}
