using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 8;
    private int currentHealth;
    private bool rotateToPlayer = false;
    private bool attackReady = true;
    private bool patrolReady = true;
    private bool withinAttackRadius = false;
    private float attackAngle = 20f;
    private float patrolRate = 5f;
    private float patrolRange = 20f;
    private float attackRadius;
    private float chaseRadius = 15f;
    private float distanceFromPlayer;
    private float attackRate = 1f;
    private float decisionRate = 0.5f;

    private Animator animator;
    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent;

    // Later this will be the player
    public Transform debugTransform;

    [SerializeField] AudioClip bulletOnMetalSFX;
    [SerializeField] RagdollHelper ragdoll;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        attackRadius = navMeshAgent.stoppingDistance;

        // Later this will be the player
        debugTransform = GameObject.Find("DebugTarget").transform;
    }

    void Update()
    {
        // Limits the decision making to optimize performance
        if (decisionRate <= 0)
        {
            HandleAction();
            decisionRate = 0.5f;
        }
        else
        {
            decisionRate -= Time.deltaTime;
        }

        // If the player is within attack range, this parameter will be true (except during the attack animation)
        if (rotateToPlayer)
        {
            // Later this will be the player
            RotateToPlayer(debugTransform);
        }

        // Update movement animation
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    private void HandleAction()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, debugTransform.position);
        
        if (distanceFromPlayer <= attackRadius)
        {
            Attack();
        }
        else if (distanceFromPlayer <= chaseRadius)
        {
            rotateToPlayer = false;
            withinAttackRadius = false;
            ChasePlayer();
        }
        else
        {
            rotateToPlayer = false;
            withinAttackRadius = false;
            Patrol();
        }
    }

    private void Attack()
    {
        // Make the enemy stop moving before attacking
        navMeshAgent.SetDestination(transform.position);
        
        // This parameter is only set to true when the players enters the attack radius
        if (!withinAttackRadius)
        {
            rotateToPlayer = true;
            withinAttackRadius = true;
        }

        // The attack will only occur when the player is within the attack angle
        if (attackReady && WithinAngle(debugTransform, attackAngle))
        {
            attackReady = false;
            // Disable the rotation towards the player during the attack animation
            rotateToPlayer = false;
            animator.SetTrigger("Stab Attack");
            StartCoroutine(attackCooldown());
        }
    }

    // Checks if the target is within the given angle
    private bool WithinAngle(Transform target, float angle)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float currentAngle = Vector3.Angle(direction, transform.forward);

        if (currentAngle <= angle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RotateToPlayer(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
    }

    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(attackRate);
        attackReady = true;

        // Check if the player still is within the attack radius
        if (withinAttackRadius)
        {
            rotateToPlayer = true;
        }
    }

    private void ChasePlayer()
    {
        // Later this will be the player
        navMeshAgent.SetDestination(debugTransform.position);
    }

    private void Patrol()
    {
        // Get and set the patrol destination
        if (patrolReady)
        {
            patrolReady = false;
            navMeshAgent.SetDestination(GetValidPatrolPoint());
            StartCoroutine(patrolCooldown());
        }
    }

    private Vector3 GetValidPatrolPoint()
    {
        // Try getting a point until it is on the ground (walkable area)
        while (true)
        {
            float randomX = Random.Range(-patrolRange, patrolRange);
            float randomZ = Random.Range(-patrolRange, patrolRange);
            Vector3 destination = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            // Check if the point is above the ground
            Vector3 destinationCheck = destination + new Vector3(0, 1, 0);
           if (Physics.Raycast(destinationCheck, -Vector3.up, 10f, ~LayerMask.NameToLayer("Ground")))
            {
                return destination;
            }
        }
    }

    IEnumerator patrolCooldown()
    {
        yield return new WaitForSeconds(patrolRate);
        patrolReady = true;
    }

    public void TakeDamage()
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

    public void PlayHardHitSFX()
    {
        audioSource.PlayOneShot(bulletOnMetalSFX);
    }

    private void Die()
    {
        // Stop running the update method
        this.enabled = false;
        ragdoll.ToggleToRagdoll(transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
