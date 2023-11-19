using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class VillagerLifeController : MonoBehaviour
{
    NavMeshAgent agent;

    public float speed;
    public string name;
    public SceneInfo playerStorage;
    public bool isInDialog = false;

    private float timeInCycle;
    private SpriteRenderer sprite;
    private float waitTimeRangeFrom = 5;
    private float waitTimeRangeTo = 20;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private Animator animator;
    private PlayerController playerController;

    private Dictionary<string, int> scheduleIndices = new Dictionary<string, int>()
    {
        {"Police", 0},
        {"Mayor", 1},
        {"Samuel", 2},
        {"Isabel", 3},
        {"Lillian", 4},
        {"Walter", 5},
    };
    private bool hasWaitedAtPatrolPoint;
    private bool isWaiting = false;
    private bool leavingScene = false;
    private int scheduleIdx = -1;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoBraking = true;
        agent.speed = speed;

        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        playerController = FindObjectOfType<PlayerController>();
        sprite = GetComponent<SpriteRenderer>();

        scheduleIdx = scheduleIndices[name];
        Destination currentDestination = playerStorage.schedules[scheduleIdx].destinations[playerStorage.schedules[scheduleIdx].curIndex];
        Debug.Log("Current destination: " + playerStorage.schedules[scheduleIdx].curIndex);
        if (SceneManager.GetActiveScene().name != currentDestination.sceneName && (IsTime(TimeOfDay.Day) || IsTime(TimeOfDay.Morning)))
        {
            DisableSprite();
        }

        GotoNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInDialog)
        {
            // Stop the villager from moving
            if (agent.velocity.magnitude > 0)
            {
                agent.velocity = Vector3.zero;
            }
            FacePlayer();

            return;
        }
        timeInCycle = TimeManager.instance.inGameTime%(playerStorage.dayDuration + playerStorage.transitionDuration + playerStorage.nightDuration + playerStorage.transitionDuration);
        
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (leavingScene)
            {
                DisableSprite();
            }
            
            if (hasWaitedAtPatrolPoint)
            {
                GotoNextPoint();
            }
            else if (!isWaiting)
            {
                StartCoroutine(Wait());
                isWaiting = true;
            }
        }

        if (IsTime(TimeOfDay.Evening) && SceneManager.GetActiveScene().name == "MainScene")
        {
            agent.destination = playerStorage.schedules[scheduleIdx].homeEntrance;
            leavingScene = true;
        }
        else if (IsTime(TimeOfDay.Night) && SceneManager.GetActiveScene().name == "MainScene")
        {
            agent.transform.position = playerStorage.schedules[scheduleIdx].homeEntrance;
            leavingScene = true;
        }
        
        // Get the current velocity of the NavMeshAgent
        Vector3 velocity = agent.velocity;
        
        // Check if the NavMeshAgent is moving
        bool isMoving = velocity.magnitude > 0;

        // Set the animation parameters
        animator.SetBool("isMoving", isMoving);
        if (isMoving)
        {
            // Normalize the velocity to get the direction of movement
            Vector3 normalizedVelocity = velocity.normalized;

            // Set the direction of movement
            Vector3 direction = new Vector3(normalizedVelocity.x, normalizedVelocity.y, 0);

            // Set the animation parameters
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }
    }

    void GotoNextPoint()
    {
        if (playerStorage.schedules[scheduleIdx].destinations.Count == 0)
            return;
        if ((IsTime(TimeOfDay.Evening) || IsTime(TimeOfDay.Night)) && SceneManager.GetActiveScene().name == "MainScene")
            return;
        leavingScene = false;
        // Get the current destination
        Destination currentDestination = playerStorage.schedules[scheduleIdx].destinations[playerStorage.schedules[scheduleIdx].curIndex];
        Debug.Log("GoToNextPoint: Should be going to: " + playerStorage.schedules[scheduleIdx].curIndex);


        // Check if the destination is in the same scene
        if (currentDestination.sceneName == SceneManager.GetActiveScene().name)
        {
            if (!sprite.enabled) EnableSprite();

            // Set the agent to go to the destination
            agent.destination = currentDestination.position;
            Debug.Log("Going to " + playerStorage.schedules[scheduleIdx].curIndex);
        }
        else
        {
            // Check if the villager is in the MainScene
            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                // Set the agent to go to the home entrance
                agent.destination = playerStorage.schedules[scheduleIdx].homeEntrance;
                leavingScene = true;
                Debug.Log("Going to home entrance");
            }
            else if (IsTime(TimeOfDay.Day) || IsTime(TimeOfDay.Morning))
            {
                agent.destination = playerStorage.schedules[scheduleIdx].homeExit;
                leavingScene = true;
                Debug.Log("Going to home exit");
            }
            else
            {
                // Set the agent to go to somewhere exclusively not in the MainScene
                while (currentDestination.sceneName == "MainScene")
                {
                    playerStorage.schedules[scheduleIdx].curIndex = Random.Range(2, playerStorage.schedules[scheduleIdx].destinations.Count);
                    currentDestination = playerStorage.schedules[scheduleIdx].destinations[playerStorage.schedules[scheduleIdx].curIndex];
                }
                agent.destination = currentDestination.position;
                Debug.Log("Going to " + playerStorage.schedules[scheduleIdx].curIndex);
            }
        }

        // Move to a random index in the schedule
        playerStorage.schedules[scheduleIdx].curIndex = Random.Range(0, playerStorage.schedules[scheduleIdx].destinations.Count);

        hasWaitedAtPatrolPoint = false;
    }

    IEnumerator Wait()
    {
        float waitTime = Random.Range(waitTimeRangeFrom, waitTimeRangeTo);
        Debug.Log("Waiting for " + waitTime + " seconds");
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        hasWaitedAtPatrolPoint = true;
    }

    enum TimeOfDay
    {
        Day,       // 0
        Evening,   // 1
        Night,     // 2
        Morning    // 3
    }

    bool IsTime(TimeOfDay marker)
    {
        TimeOfDay mark = TimeOfDay.Day;
        if (timeInCycle < playerStorage.dayDuration)
        {
            mark = TimeOfDay.Day;
        }
        else if (timeInCycle < playerStorage.dayDuration + playerStorage.transitionDuration)
        {
            mark = TimeOfDay.Evening;
        }
        else if (timeInCycle < playerStorage.dayDuration + playerStorage.transitionDuration + playerStorage.nightDuration)
        {
            mark = TimeOfDay.Night;
        }
        else
        {
            mark = TimeOfDay.Morning;
        }

        return mark == marker;
    }

    void DisableSprite()
    {
        sprite.enabled = false;
        boxCollider.enabled = false;
        circleCollider.enabled = false;
    }

    void EnableSprite()
    {
        sprite.enabled = true;
        boxCollider.enabled = true;
        circleCollider.enabled = true;
    }

    void FacePlayer()
    {
        // Face the player
        Vector3 directionToPlayer = playerController.transform.position - transform.position;
        directionToPlayer.Normalize();

        animator.SetBool("isMoving", false);
        animator.SetFloat("moveX", directionToPlayer.x);
        animator.SetFloat("moveY", directionToPlayer.y);
    }
}
