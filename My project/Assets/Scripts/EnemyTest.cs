using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class enemy1 : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public float health;

    public LayerMask whatIsGround, whatIsPlayer;


    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // Jump attack
    public float jumpForce = 7f;
    public float forwardForce = 8f;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private Rigidbody rb;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (agent == null || !agent.enabled || !agent.isOnNavMesh)
            return;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkpoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkpoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        //Make sure enemy does not move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().isKinematic = false;
            }
            alreadyAttacked = true;

            // Stop NavMeshAgent so physics can take over
            agent.enabled = false;

            transform.LookAt(player);

            // Calculate jump direction
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 jumpVector = direction * forwardForce + Vector3.up * jumpForce;

            // Apply jump force
            rb.AddForce(jumpVector, ForceMode.Impulse);

            // Reset after delay
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        // Re-enable agent after landing delay
        StartCoroutine(ReenableAgentAfterDelay(1f));
    }
    private IEnumerator ReenableAgentAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Wait until the enemy is grounded and near a NavMesh surface
        yield return new WaitUntil(() => IsGrounded() && IsOnNavMesh());

        agent.enabled = true;
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    private bool IsGrounded() =>
        Physics.Raycast(transform.position, Vector3.down, 1.2f, whatIsGround);

    private bool IsOnNavMesh()
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }


}