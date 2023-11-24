using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class VillagerLifeController : MonoBehaviour
{
    NavMeshAgent agent;

    public float speed;
    public string villagerName;
    public SceneInfo playerStorage;
    public bool isInDialog = false;

    private float timeInCycle;
    private SpriteRenderer sprite;
    private float waitTimeRangeFrom = 5;
    private float waitTimeRangeTo = 20;
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
    private int prevIdx = -1;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoBraking = true;
        agent.speed = speed;

        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        playerController = FindObjectOfType<PlayerController>();
        sprite = GetComponent<SpriteRenderer>();

        scheduleIdx = scheduleIndices[villagerName];
        timeInCycle = TimeManager.instance.inGameTime%(playerStorage.dayDuration + playerStorage.transitionDuration + playerStorage.nightDuration + playerStorage.transitionDuration);

        if (!IsPoliceAndIntroduced()) // This is to prevent police from moving in the beginning
        {
            GotoNextPoint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInDialog)
        {
            // Stop the villager from moving
            if (!agent.isStopped) agent.isStopped = true;
            FacePlayer();

            return;
        }
        if (agent.isStopped) agent.isStopped = false;
        timeInCycle = TimeManager.instance.inGameTime%(playerStorage.dayDuration + playerStorage.transitionDuration + playerStorage.nightDuration + playerStorage.transitionDuration);
        
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (leavingScene)
            {
                DisableSprite();
            }
            else
            {
                EnableSprite();
            }
            
            if (hasWaitedAtPatrolPoint && !IsPoliceAndIntroduced())
            {
                GotoNextPoint();
            }
            else if (!isWaiting)
            {
                StartCoroutine(Wait());
                isWaiting = true;
            }
        }
        else if (!CanGoOutside() && SceneManager.GetActiveScene().name == "MainScene" && !leavingScene)
        {
            GotoNextPoint();
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

    // All cases:
    // 1. Scene = MainScene and Villager Destination = Home and Time = Day -> Go to Home and then disable
    // 2. Scene = MainScene and Villager Destination = Home and Time = Night -> Go to Home and then disable
    // 3. Scene = MainScene and Villager Destination = MainScene and Time = Day -> Go to Destination
    // 4. Scene = MainScene and Villager Destination = MainScene and Time = Night -> Go to Home and then disable
    // 5. Scene = Home and Villager Destination = MainScene and Time = Day -> Go to Door and then disable
    // 6. Scene = Home and Villager Destination = MainScene and Time = Night -> Re-roll Destination
    // 7. Scene = Home and Villager Destination = Home and Time = Day -> Go to Destination
    // 8. Scene = Home and Villager Destination = Home and Time = Night -> Go to Destination
    void GotoNextPoint()
    {
        hasWaitedAtPatrolPoint = false;
        leavingScene = false;
        Destination currentDestination = playerStorage.schedules[scheduleIdx].destinations[playerStorage.schedules[scheduleIdx].curIndex];
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if (!CanGoOutside() || currentDestination.sceneName != "MainScene")
            {
                agent.destination = playerStorage.schedules[scheduleIdx].homeEntrance;
                leavingScene = true;
            }
            else
            {
                agent.destination = currentDestination.position;
            }
        }
        else
        {
            if (currentDestination.sceneName == "MainScene")
            {
                if (!CanGoOutside())
                {
                    // Re-roll
                    while (currentDestination.sceneName == "MainScene")
                    {
                        playerStorage.schedules[scheduleIdx].curIndex = Random.Range(2, playerStorage.schedules[scheduleIdx].destinations.Count);
                        currentDestination = playerStorage.schedules[scheduleIdx].destinations[playerStorage.schedules[scheduleIdx].curIndex];
                    }
                    // Go to destination guranteed to be in home
                    agent.destination = currentDestination.position;
                }
                else
                {
                    agent.destination = playerStorage.schedules[scheduleIdx].homeExit;
                    leavingScene = true;
                }
            }
            else
            {
                agent.destination = currentDestination.position;
            }
        }
    }

    public int GetCurrentLocation()
    {
        // If agent is moving,then their velocity is greater than 0
        // thus, they aren't at any location, they're en-route
        if (agent.velocity.magnitude > 0)
        {
            return -1;
        }
        else
        {
            return prevIdx;
        }
    }

    IEnumerator Wait()
    {
        float waitTime = Random.Range(waitTimeRangeFrom, waitTimeRangeTo);
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        hasWaitedAtPatrolPoint = true;
        // Roll a new destination
        int startPos = (!CanGoOutside()) ? 2 : 0;
        playerStorage.schedules[scheduleIdx].curIndex = Random.Range(startPos, playerStorage.schedules[scheduleIdx].destinations.Count);
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

    bool CanGoOutside()
    {
        return IsTime(TimeOfDay.Day) || IsTime(TimeOfDay.Morning);
    }

    void DisableSprite()
    {
        sprite.enabled = false;
        circleCollider.enabled = false;
    }

    void EnableSprite()
    {
        sprite.enabled = true;
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

    bool IsPoliceAndIntroduced()
    {
        return villagerName=="Police" && !playerStorage.dialogueRead[0].baseDialogue;
    }
}
