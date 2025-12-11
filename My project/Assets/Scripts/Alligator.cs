using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemy5 : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public float health;

    public float SpeedUp;

    public float SlowDown;

    public Transform Spawnpoint;

    public AudioSource damagetakenSpeaker;

    public GameObject drop;

    public GameObject projectile;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float distance;
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    PlayerController playerController;
    private IEnumerator dropper()
    {
        playerController.Kills += 1;

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
        playerController = GameObject.FindGameObjectWithTag("player")?.GetComponent<PlayerController>();
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
        damagetakenSpeaker = GameObject.FindGameObjectWithTag("damageaudio")?.GetComponent<AudioSource>();
    }
    private void Update()
    {
        //Check for sight and attack range
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
        agent.speed = SlowDown;
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
        //Make sure enemy charges

        agent.speed = SpeedUp;
        agent.SetDestination(player.position);
        if (!alreadyAttacked)
        {
            ///Attack code here




            ///

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
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
            TakeDamage(playerController.damage);

        }
    }



}