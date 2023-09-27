using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAI : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    int currentPointIndex;

    NavMeshAgent agent;

    public float speed;
    public float waitTime;

    bool hasWaitedAtPatrolPoint;
    bool isWaiting = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoBraking = false;
        agent.speed = speed;

        GotoNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (hasWaitedAtPatrolPoint == false)
            {
                if (!isWaiting)
                {
                    isWaiting = true;
                    StartCoroutine(Wait());
                }
            } else
            {
                GotoNextPoint();
            }
        }
    }

    void GotoNextPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.destination = patrolPoints[currentPointIndex].position;

        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;

        hasWaitedAtPatrolPoint = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        hasWaitedAtPatrolPoint = true;
    }
}
