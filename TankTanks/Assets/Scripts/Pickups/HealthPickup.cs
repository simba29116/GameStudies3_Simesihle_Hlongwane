using UnityEngine;


public class HealthPickup : PickUpBase
{
    [SerializeField] private int healthAmount = 25;

    protected override void ApplyEffect(GameObject player)
    {
        playerHealth health = player.GetComponent<playerHealth>();

        if (health == null)
            return;

        health.currentHealth += healthAmount;

        if (health.currentHealth > health.maxHealth)
        {
            health.currentHealth = health.maxHealth;
        }
    }
}
