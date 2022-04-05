using UnityEngine;
using UnityEngine.AI;

public class EnemyShoot : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform player2;
    public LayerMask whatisGround, whatisPlayer, whatisPlayer2;

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
    public bool playerInSightRange, playerInAttackRange, player2InSightRange;
    public static bool playerHidden = false;
    public GameObject spottedIcon, lostIcon;
    bool played;

    public Animator anim;

    public static bool chaseTheSwede = false;
    bool invokePlayed;
    bool ePressed = false;
    public Player2Movement swedeScript;

    float timer;

    public AudioSource hmm;
    private void Start()
    {
        player = GameObject.Find("Player1").transform;
        playerScript = GameObject.Find("Player1").GetComponent<PlayerMovement>();
        player2 = GameObject.Find("Player2").transform;
        swedeScript = GameObject.Find("Player2").GetComponent<Player2Movement>();
        agent = GetComponent<NavMeshAgent>();

        spottedIcon.SetActive(false);
        lostIcon.SetActive(false);

        timer = 10f;
    }

    private void Update()
    {
        //Check for sight and capture range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatisPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatisPlayer);

        player2InSightRange = Physics.CheckSphere(transform.position, sightRange, whatisPlayer2);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerHidden == false)
        {
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }
        if (playerHidden == true) Patroling();

        if (player2InSightRange)
        {
            if (ePressed == false && Input.GetKeyDown(KeyCode.E))
            {
                ePressed = true;
                chaseTheSwede = true;
            }
        }

        if (chaseTheSwede == true)
        {
            ChaseSwede();
        }
    }

    void Patroling()
    {
        if (played == false)
        {
            hmm.Play();
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

        if (distanceToWalkPoint.magnitude > 2f)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                walkPointSet = false;
                anim.SetBool("Walking", false);
                timer = 10f;
            }
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


    void ChaseSwede()
    {
        spottedIcon.SetActive(false);
        lostIcon.SetActive(true);

        agent.SetDestination(player2.position);

        if(invokePlayed == false)
        {
            invokePlayed = true;
            hmm.Play();
            Invoke("StopSwedeChase", 6f);
        }

        anim.SetBool("Walking", true);
    }
    void StopSwedeChase()
    {
        ePressed = false;
        chaseTheSwede = false;
        spottedIcon.SetActive(false);
        lostIcon.SetActive(false);
        invokePlayed = false;
    }

    void AttackPlayer()
    {
        //Make sure enemy doesnt move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke("Attack", 0.45f);
            anim.SetBool("Walking", false);
            anim.SetTrigger("Grenade");
        }
    }

    void Attack()
    {
            //Attack Code
            Rigidbody rb = Instantiate(projectile, attackPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 7f, ForceMode.Impulse);
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);
            //

            Invoke(nameof(ResetAttack), timeBetweenAttacks);

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
