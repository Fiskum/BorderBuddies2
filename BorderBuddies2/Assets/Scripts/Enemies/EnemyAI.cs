using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
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

    //States
    public float sightRange, captureRange;
    public bool playerInSightRange, playerInCaptureRange;

    private void Start()
    {
        player = GameObject.Find("Player1").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and capture range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatisPlayer);
        playerInCaptureRange = Physics.CheckSphere(transform.position, captureRange, whatisPlayer);

        if (!playerInSightRange && !playerInCaptureRange) Patroling();
        if (playerInSightRange && !playerInCaptureRange) ChasePlayer();
        if (playerInSightRange && playerInCaptureRange) CapturePlayer();
    }

    void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
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
        agent.SetDestination(player.position);
    }

    void CapturePlayer()
    {
        //Make sure enemy doesnt move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        playerScript.Captured();
    }
}
