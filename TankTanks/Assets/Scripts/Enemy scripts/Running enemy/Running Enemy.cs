using UnityEngine;

public class RunningEnemy : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public float speed; // Speed of the enemy
    public Rigidbody rb;
    public float detectionRange; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            transform.LookAt(player.gameObject.transform);

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }



        



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

