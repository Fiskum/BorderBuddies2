using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
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

    //States
    public float sightRange, captureRange;
    public bool playerInSightRange, playerInCaptureRange, player2InSightRange;
    public static bool playerHidden = false;
    public GameObject spottedIcon, lostIcon;
    bool played;

    public Animator anim;
    bool animPlayed = false;

    public static bool chaseTheSwede = false;
    bool invokePlayed;
    public Player2Movement swedeScript;
    private void Start()
    {
        player = GameObject.Find("Player1").transform;
        player2 = GameObject.Find("Player2").transform;
        agent = GetComponent<NavMeshAgent>();

        spottedIcon.SetActive(false);
        lostIcon.SetActive(false);
    }

    private void Update()
    {
        //Check for sight and capture range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatisPlayer);
        playerInCaptureRange = Physics.CheckSphere(transform.position, captureRange, whatisPlayer);

        player2InSightRange = Physics.CheckSphere(transform.position, sightRange, whatisPlayer2);

        if (!playerInSightRange && !playerInCaptureRange) Patroling();
        if(playerHidden == false)
            {
                if (playerInSightRange && !playerInCaptureRange) ChasePlayer();
                if (playerInSightRange && playerInCaptureRange) CapturePlayer();
            }
        if (playerHidden == true) Patroling();

        if (player2InSightRange)
        {
            if (swedeScript.whistlePlayed == false && Input.GetKeyDown(KeyCode.E))
            {
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
        if(played == false)
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
    }

    void ChaseSwede()
    {
        spottedIcon.SetActive(false);
        lostIcon.SetActive(true);

        agent.SetDestination(player2.position);

        if (invokePlayed == false)
        {
            invokePlayed = true;
            Invoke("StopSwedeChase", 3f);
        }

        anim.SetBool("Walking", true);
    }
    void StopSwedeChase()
    {
        chaseTheSwede = false;
        spottedIcon.SetActive(false);
        lostIcon.SetActive(false);
        invokePlayed = false;
    }

    void CapturePlayer()
    {
        //Make sure enemy doesnt move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        playerScript.Captured();

        if(animPlayed == false)
        {
            animPlayed = true;
            anim.SetTrigger("Laughing");
        }      
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
