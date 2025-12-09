using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy3 : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public float health;
    public GameObject drop;
    public GameObject attack;
    public LayerMask whatIsGround, whatIsPlayer;
    public Transform Spawnpoint;
    public Transform Spawnpoint1;
    public Transform Spawnpoint2;
    public AudioSource damagetakenSpeaker;
    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    public float timeBetweenAttacks = 2f;
    bool alreadyAttacked;

    //Jump Attack
    public float jumpForce = 6.5f;
    public float forwardForce = 8f;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private Rigidbody rb;
    private int enemyLayer;

    private IEnumerator dropper()
    {
        int random = Random.Range(1, 100);

        Debug.Log("RANDOM WAS  " + random);

        if (random >= 80)
        {
            Debug.Log("GOOD LUCK");
            Instantiate(drop, Spawnpoint.position, Spawnpoint.rotation);
            Destroy(gameObject);
        }

        if (random < 80)
        {
            Debug.Log("BAD LUCK");
            Destroy(gameObject);
        }


        yield return null;



    }
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("player")?.transform;

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        // Store this enemy layer to ignore it in grounding
        enemyLayer = gameObject.layer;

        // Make sure Rigidbody doesn’t rotate (keeps physics stable)
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if (agent == null)
            return;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!agent.enabled) return;

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkpoint = transform.position - walkPoint;

        if (distanceToWalkpoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z + randomZ
        );

        if (Physics.Raycast(walkPoint, -transform.up, 3f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        if (agent.enabled)
            agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked && agent.enabled)
        {
            alreadyAttacked = true;

            agent.enabled = false;   // Turn off navmesh movement

            StartCoroutine(Laser());




            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        StartCoroutine(ReenableAgent());
    }

    IEnumerator Laser()
    {
        yield return new WaitForSeconds(0.5f);

        Instantiate(attack, Spawnpoint1.position, Spawnpoint.rotation);

        yield return new WaitForSeconds(1f);

        Instantiate(attack, Spawnpoint2.position, Spawnpoint.rotation);
       
        yield return new WaitForSeconds(0.5f);

        Instantiate(attack, Spawnpoint1.position, Spawnpoint.rotation);
        
        yield return new WaitForSeconds(1f);

        Instantiate(attack, Spawnpoint2.position, Spawnpoint.rotation);

        StartCoroutine(ReenableAgentAttack());
    }
    private IEnumerator ReenableAgent()
    {
        // Wait until grounded on actual ground (not another enemy)
        yield return new WaitUntil(() => IsGroundedProper());

        // Wait a moment for stability
        yield return new WaitForSeconds(0.25f);

        rb.isKinematic = true;

        // Try to snap to nearest navmesh point
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }

        agent.enabled = true;
        alreadyAttacked = false;
    }
    private IEnumerator ReenableAgentAttack()
    {
        // Wait until grounded on actual ground (not another enemy)
        yield return new WaitUntil(() => IsGroundedProper());

        // Wait a moment for stability
        yield return new WaitForSeconds(0.25f);

        rb.isKinematic = true;

        // Try to snap to nearest navmesh point
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }

        agent.enabled = true;
        alreadyAttacked = true;
    }

    private bool IsGroundedProper()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, whatIsGround))
        {
            return true;
        }

        return false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        StartCoroutine(dropper());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack1"))
        {
            damagetakenSpeaker.Play();
            TakeDamage(0.5f);
          
        }
        
    }



}
