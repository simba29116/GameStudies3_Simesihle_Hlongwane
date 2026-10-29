using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class playerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Image HealthBar;
    public FadeCode fadeCode; 
    public GameObject gameOverScreen; 

    private bool isDead = false;

    void Start()
    {
        gameOverScreen.SetActive(false);
        currentHealth = maxHealth;

    }

     
    void Die()
    {
        
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log("Player has died.");
            // add game over screen
            gameOverScreen.SetActive(true);
            StartCoroutine(fadeCode.FadeIn());
        }
    }
    private void FixedUpdate()
    {
        HealthBar.fillAmount = Mathf.Clamp((float)currentHealth / maxHealth, 0, 1 );
        Die();
    }
}
