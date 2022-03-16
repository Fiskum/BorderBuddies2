using UnityEngine;
using UnityEngine.AI;

public class EnemyShoot : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatisGround, whatisPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Capture
    public PlayerMovement playerScript;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public GameObject attackPoint;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public static bool playerHidden = false;
    public GameObject spottedIcon, lostIcon;
    bool played;

    public Animator anim;

    private void Start()
    {
        player = GameObject.Find("Player1").transform;
        agent = GetComponent<NavMeshAgent>();

        spottedIcon.SetActive(false);
        lostIcon.SetActive(false);
    }

    private void Update()
    {
        //Check for sight and capture range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatisPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatisPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerHidden == false)
        {
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }
        if (playerHidden == true) Patroling();
    }

    void Patroling()
    {
        if (played == false)
        {
            spottedIcon.SetActive(false);
            lostIcon.SetActive(true);
            Invoke("IconOff", 2f);
            played = true;
        }

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        anim.SetBool("Walking", true);

        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 2f)
        {
            walkPointSet = false;

            anim.SetBool("Walking", false);
        }
            
    }

    void IconOff()
    {
        lostIcon.SetActive(false);
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatisGround))
            walkPointSet = true;
    }

    void ChasePlayer()
    {
        spottedIcon.SetActive(true);
        lostIcon.SetActive(false);

        agent.SetDestination(player.position);
        played = false;

        anim.SetBool("Walking", true);
    }

    void AttackPlayer()
    {
        //Make sure enemy doesnt move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack Code
            Rigidbody rb = Instantiate(projectile, attackPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 7f, ForceMode.Impulse);
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);
            //

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            anim.SetBool("Walking", false);

            anim.SetTrigger("Grenade");
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
