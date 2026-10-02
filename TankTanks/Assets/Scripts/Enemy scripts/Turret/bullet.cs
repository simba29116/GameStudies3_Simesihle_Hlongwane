using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 5f;
    public float lifetime = 5f;
    public LayerMask hitLayers;
    private float lifeTimer;

    private void OnEnable()
    {
        lifeTimer = lifetime;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & hitLayers) != 0)
        {
            gameObject.SetActive(false);
        }
    }
}
