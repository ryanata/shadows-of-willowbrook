using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TownNpc : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    NavMeshAgent agent;

    public float speed;
    public Transform home;

    public GameObject globalLight;
    private DayLightCycle dayLightCycleScript;
    private float returnHomeTime = 210f;
    
    private SpriteRenderer sprite;

    private float waitTimeRangeFrom = 5;
    private float waitTimeRangeTo = 30;
    private int currentPointIndex;

    private bool hasWaitedAtPatrolPoint;
    private bool isWaiting = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoBraking = true;
        agent.speed = speed;

        dayLightCycleScript = globalLight.GetComponent<DayLightCycle>();
        sprite = GetComponent<SpriteRenderer>();

        GotoNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (dayLightCycleScript.timeInDay >= returnHomeTime)
        {
            agent.SetDestination(home.position);
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                sprite.enabled = false;
            }
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (hasWaitedAtPatrolPoint == false)
            {
                if (!isWaiting)
                {
                    isWaiting = true;
                    StartCoroutine(Wait());
                }
            }
            else
            {
                if (!sprite.enabled)
                {
                    sprite.enabled = true;
                }
                GotoNextPoint();
            }
        }
    }

    void GotoNextPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.SetDestination(patrolPoints[currentPointIndex].position);

        currentPointIndex = Random.Range(0, patrolPoints.Length-1);

        hasWaitedAtPatrolPoint = false;
    }

    IEnumerator Wait()
    {
        float waitTime = Random.Range(waitTimeRangeFrom, waitTimeRangeTo);
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        hasWaitedAtPatrolPoint = true;
    }
}