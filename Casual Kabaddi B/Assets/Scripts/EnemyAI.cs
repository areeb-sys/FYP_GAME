using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator anim;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;


    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isIdle", true);
    }
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange && playerInSightRange)
            AttackPlayer();
        else if (playerInSightRange)
            ChasePlayer();
        else
            StopChasingPlayer();
    }

    private void ChasePlayer()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", true);
        //Debug.Log("Animation started");

        agent.SetDestination(player.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // Stop moving and play idle animation
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", true);
        }
    }

    private void StopChasingPlayer()
    {
        // Stop moving and play idle animation
        agent.SetDestination(transform.position);
        anim.SetBool("isWalking", false);
        anim.SetBool("isIdle", true);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack the player
            anim.SetTrigger("attack");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
