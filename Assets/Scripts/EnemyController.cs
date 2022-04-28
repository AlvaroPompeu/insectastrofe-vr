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
    private float attackAngle = 30f;
    private float patrolRate = 5f;
    private float patrolRange = 20f;
    private float attackRadius;
    public float chaseRadius;
    private float normalChaseRadius = 15f;
    private float enragedChaseRadius = 35f;
    private float enrageTimer = 0;
    private float distanceFromPlayer;
    private float attackRate = 1f;
    private float decisionRate = 0.5f;

    private Animator animator;
    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent;
    private Transform playerBody;
    private MetalonHorn horn;

    [SerializeField] AudioClip bulletOnMetalSFX;
    [SerializeField] RagdollHelper ragdoll;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        attackRadius = navMeshAgent.stoppingDistance;
        playerBody = GameObject.Find("PlayerBody").transform;
        horn = GetComponentInChildren<MetalonHorn>();

        chaseRadius = normalChaseRadius;
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

        // Manage the enrage chase radius
        if (enrageTimer <= 0)
        {
            // Reset the chase radius
            chaseRadius = normalChaseRadius;
        }
        else
        {
            enrageTimer -= Time.deltaTime;
        }


        // If the player is within attack range, this parameter will be true (except during the attack animation)
        if (rotateToPlayer)
        {
            RotateToPlayer(playerBody);
        }

        // Update movement animation
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    private void HandleAction()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, playerBody.position);
        
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
        if (attackReady && WithinAngle(playerBody, attackAngle))
        {
            attackReady = false;
            // Disable the rotation towards the player during the attack animation
            rotateToPlayer = false;
            animator.SetTrigger("Stab Attack");
            //Enable the collider so the horn can hit the player
            horn.OpenCollider();
            StartCoroutine(attackCooldown());
        }
    }

    // Checks if the target is within the given angle
    private bool WithinAngle(Transform target, float angle)
    {
        // Discard the Y axis when calculating the angle
        Vector3 direction = (new Vector3(target.position.x, 0, target.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
        float currentAngle = Vector3.Angle(direction, new Vector3(transform.forward.x, 0, transform.forward.z));

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
        navMeshAgent.SetDestination(playerBody.position);
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
        // Enrage the enemy
        Enrage();

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
        // Enrage the enemy
        Enrage();

        audioSource.PlayOneShot(bulletOnMetalSFX);
    }
    
    private void Enrage()
    {
        chaseRadius = enragedChaseRadius;
        enrageTimer = 5f;
    }

    private void Die()
    {
        // Increment the kill count
        GameManager.Instance.metalonKillCount++;

        // Stop running the update method
        this.enabled = false;
        ragdoll.ToggleToRagdoll(transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
