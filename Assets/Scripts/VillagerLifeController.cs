using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class VillagerLifeController : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    NavMeshAgent agent;

    public float speed;
    public Transform home;
    public SceneInfo playerStorage;

    private float timeInCycle;
    private SpriteRenderer sprite;
    private float waitTimeRangeFrom = 5;
    private float waitTimeRangeTo = 20;
    private int currentPointIndex;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;

    private bool hasWaitedAtPatrolPoint;
    private bool isWaiting = false;
    private bool leavingScene = false;
    private bool onMainScene = true;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoBraking = true;
        agent.speed = speed;

        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        onMainScene = SceneManager.GetActiveScene().name == "MainScene";

        GotoNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        timeInCycle = TimeManager.instance.inGameTime%(playerStorage.dayDuration + playerStorage.transitionDuration + playerStorage.nightDuration + playerStorage.transitionDuration);
        
        if (IsTime(TimeOfDay.Night) && onMainScene)
        {
            if (sprite.enabled)
            {
                // Teleport to home
                transform.position = home.position;
                DisableSprite();
            }
            return;
        }

        if (IsTime(TimeOfDay.Evening) && onMainScene)
        {
            agent.SetDestination(home.position);
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                // Disappear the sprite
                DisableSprite();
            }
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (hasWaitedAtPatrolPoint == false)
            {
                if (!isWaiting)
                {
                    isWaiting = true;
                    if (leavingScene)
                    {
                        DisableSprite();
                        leavingScene = false;
                    }
                    StartCoroutine(Wait());
                }
            }
            else
            {
                if (!sprite.enabled && currentPointIndex < patrolPoints.Length)
                {
                    EnableSprite();
                }
                GotoNextPoint();
            }
        }
    }

    void GotoNextPoint()
    {
        Debug.Log("Going to next point.");
        if (patrolPoints.Length == 0)
            return;
        
        bool cantLeaveHouse = !onMainScene && (IsTime(TimeOfDay.Evening) || IsTime(TimeOfDay.Night));
        if (currentPointIndex >= patrolPoints.Length && cantLeaveHouse)
        {
            currentPointIndex = Random.Range(0, patrolPoints.Length);
        }

        if (currentPointIndex < patrolPoints.Length)
        {
            agent.SetDestination(patrolPoints[currentPointIndex].position);
            Debug.Log("Going to patrol point " + currentPointIndex);
        }
        else
        {
            agent.SetDestination(home.position);
            leavingScene = true;
            Debug.Log("Going home");
        }

        currentPointIndex = Random.Range(0, patrolPoints.Length+1);

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
}
