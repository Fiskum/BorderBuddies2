using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    Transform player2;

    [Header("Layers")]
    public LayerMask whatisGround;
    public LayerMask whatisPlayer1;
    public LayerMask whatisPlayer2;

    //Patrolling
    Vector3 walkPoint;
    bool walkPointSet;

    //PlayerScripts
    PlayerMovement playerScript;
    Player2Movement player2Script;

    //Parameters
    [Header("Parameters")]
    [Range(1f, 20f)]
    public float walkPointDistance = 10f;
    [Range(1f, 20f)]
    public float sightRange = 12f;
    [Range(1f, 20f)]
    public float attackRange = 4f;

    //States
    bool playerInSightRange, playerInAttackRange, player2InSightRange;
    public static bool playerHidden = false;

    [Header("Popup Icons")]
    public GameObject spottedIcon;
    public GameObject lostIcon;

    bool played;
    int patrolSoundTrigger;
    bool voicePlayed;

    Animator enemyBodyAnim;
    bool animPlayed = false;

    public static bool chaseTheSwede = false;
    bool invokePlayed;
    bool ePressed = false;
    bool soundDone;
    bool soundFinshed;

    float timer;

    AudioSource audioSource;

    [Header("Voice Lines")]
    public AudioClip[] losePlayerSounds;
    public AudioClip[] spotPlayerSounds;
    public AudioClip[] patrolSounds;
    public AudioClip[] distractedSounds;
    public AudioClip[] captureSounds;
    public AudioClip[] stuckSounds;

    private void Awake()
    {
        enemyBodyAnim = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        player = GameObject.Find("Player1").transform;
        playerScript = GameObject.Find("Player1").GetComponent<PlayerMovement>();
        player2 = GameObject.Find("Player2").transform;
        player2Script = GameObject.Find("Player2").GetComponent<Player2Movement>();
        agent = GetComponent<NavMeshAgent>();

        spottedIcon.SetActive(false);
        lostIcon.SetActive(false);

        audioSource = GetComponent<AudioSource>();

        timer = 10f;
    }

    private void Update()
    {
        //Check for sight and capture range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatisPlayer1);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatisPlayer1);

        player2InSightRange = Physics.CheckSphere(transform.position, sightRange, whatisPlayer2);

        if (!playerInSightRange && !playerInAttackRange && EnemyAlerter.playerSpotted == false) Patroling();
        if(playerHidden == false)
            {
                if (playerInSightRange && !playerInAttackRange) ChasePlayer();
                if (playerInSightRange && playerInAttackRange) CapturePlayer();
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

        if (EnemyAlerter.playerSpotted == true && !playerInAttackRange)
        {
            ChasePlayer();
        }
    }

    void Patroling()
    {
        if(played == false)
        {
            if(soundDone == false) 
            { 
                soundDone = true;
                audioSource.clip = losePlayerSounds[Random.Range(0, losePlayerSounds.Length)];
                audioSource.PlayOneShot(audioSource.clip);
                Invoke("SoundUndone", 2f);
            }
            spottedIcon.SetActive(false);
            lostIcon.SetActive(true);
            Invoke("IconOff", 2f);
            played = true;   
        }

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        enemyBodyAnim.SetBool("Walking", true);

        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 2f)
        {
            timer = 10f;
            walkPointSet = false;

            patrolSoundTrigger = Random.Range(0, 5);

            if (patrolSoundTrigger == 3)
            {
                patrolSoundTrigger = 1;
                audioSource.clip = patrolSounds[Random.Range(0, patrolSounds.Length)];
                audioSource.PlayOneShot(audioSource.clip);
            }

            enemyBodyAnim.SetBool("Walking", false);
        }

        if (distanceToWalkPoint.magnitude > 2f)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                audioSource.clip = stuckSounds[Random.Range(0, stuckSounds.Length)];
                audioSource.PlayOneShot(audioSource.clip);
                walkPointSet = false;
                enemyBodyAnim.SetBool("Walking", false);
                timer = 10f;
            }
        }
    }
    

    void IconOff()
    {
        lostIcon.SetActive(false);
    }

    void SoundUndone()
    {
        soundDone = false;
        voicePlayed = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointDistance, walkPointDistance);
        float randomX = Random.Range(-walkPointDistance, walkPointDistance);

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

        if (!voicePlayed)
        {
            audioSource.clip = spotPlayerSounds[Random.Range(0, spotPlayerSounds.Length)];
            audioSource.PlayOneShot(audioSource.clip);
            voicePlayed = true;
        }
    }

    void ChaseSwede()
    {
        spottedIcon.SetActive(false);
        lostIcon.SetActive(true);

        agent.SetDestination(player2.position);

        if (invokePlayed == false)
        {
            invokePlayed = true;
            audioSource.clip = distractedSounds[Random.Range(0, distractedSounds.Length)];
            audioSource.PlayOneShot(audioSource.clip);
            Invoke("StopSwedeChase", 6f);
        }

        enemyBodyAnim.SetBool("Walking", true);
    }
    void StopSwedeChase()
    {
        ePressed = false;
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
            audioSource.clip = captureSounds[Random.Range(0, captureSounds.Length)];
            audioSource.PlayOneShot(audioSource.clip);
            enemyBodyAnim.SetTrigger("Laughing");
        }      
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
