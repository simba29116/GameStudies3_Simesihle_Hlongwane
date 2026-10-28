using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class playerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Image HealthBar;
    void Start()
    {
        currentHealth = maxHealth;
    }

     
    void Die()
    {
        
        if (currentHealth <= 0)
        {
            Debug.Log("Player has died.");
            // add game over screen
        }
    }
    private void FixedUpdate()
    {
        HealthBar.fillAmount = Mathf.Clamp((float)currentHealth / maxHealth, 0, 1 );
        Die();
    }
}
