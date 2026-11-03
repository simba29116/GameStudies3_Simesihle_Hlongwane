using UnityEngine;


public abstract class PickUpBase : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private float lifetime = 30f;

    protected virtual void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        ApplyEffect(other.gameObject);

        Destroy(gameObject);
    }

    protected abstract void ApplyEffect(GameObject player); 
}
