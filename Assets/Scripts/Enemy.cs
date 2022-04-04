using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int maxHealth = 8;
    private int currentHealth;
    private bool isDead = false;

    private Animator animator;
    private AudioSource audioSource;

    [SerializeField] AudioClip bulletOnMetalSFX;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void TakeDamage()
    {
        if (!isDead)
        {
            // Each pellet does 1 damage
            currentHealth--;
            if (currentHealth == 0)
            {
                Die();
            }
            // Play the hit animation
            else
            {
                animator.SetTrigger("Take Damage");
            }
        }
    }

    public void PlayHardHitSFX()
    {
        audioSource.PlayOneShot(bulletOnMetalSFX);
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        Destroy(gameObject, 20f);
        isDead = true;
    }
}
