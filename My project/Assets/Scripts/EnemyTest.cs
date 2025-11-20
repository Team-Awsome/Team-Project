using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemy1 : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public float health;
    public GameObject drop;
    public LayerMask whatIsGround, whatIsPlayer;
    public Transform Spawnpoint;

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
            rb.isKinematic = false;  // Enable physics

            // Calculate jump velocity
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 jumpVector = direction * forwardForce + Vector3.up * jumpForce;

            rb.AddForce(jumpVector, ForceMode.Impulse);

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        StartCoroutine(ReenableAgent());
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

    private bool IsGroundedProper()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, whatIsGround))
        {
            return true;
        }

        return false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Instantiate(drop, Spawnpoint.position, Spawnpoint.rotation);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack1"))
        {
            TakeDamage(3);
          
        }
    }



}
