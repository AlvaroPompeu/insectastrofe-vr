using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    private float maxHealth = 5;
    private float currentHealth;
    private bool isDead = false;
    
    // Time to heal after not taking any damage
    private float healInterval = 5f;
    private float healTimer = 0;
    
    // Healing per second
    private float healRate = 1f;

    [SerializeField] CanvasGroup damageCanvas;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (healTimer <= 0 && !isDead)
        {
            Heal();
        }
        else
        {
            healTimer -= Time.deltaTime;
        }
    }

    private void Heal()
    {
        currentHealth += healRate * Time.deltaTime;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateDamageCanvas();
    }

    public void TakeDamage()
    {
        if (!isDead)
        {
            // Apply damage and reset the heal timer
            currentHealth--;
            healTimer = healInterval;
            UpdateDamageCanvas();
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    // Adjust the red according to the player missing health percentage
    private void UpdateDamageCanvas()
    {
        float playerMissingHealthPercentage = (maxHealth - currentHealth) / maxHealth;
        damageCanvas.alpha = playerMissingHealthPercentage * 0.8f;

        // Limit the alpha
        if (damageCanvas.alpha > 0.8f)
        {
            damageCanvas.alpha = 0.8f;
        }
    }

    private void Die()
    {
        isDead = true;

        // Display GAME OVER screen
    }
}
