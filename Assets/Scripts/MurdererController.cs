using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MurdererController : MonoBehaviour
{
    public PlayerController playerController;
    public float killRange = 1.5f;
    public float killSpeed = 3f;
    public float escapeRange = 8f;
    public float escapeSpeed = 8f;
    public Vector3 nightSpawnPoint;

    private NavMeshAgent agent;
    private Transform target;
    private bool isEscaping = false;
    private float baseSpeed;
    private float escapeStart;

    private void Start()
    {
        target = playerController.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        baseSpeed = killSpeed;
    }

    private void Update()
    {
        if (playerController.isDead) return;
        updateSpeed();
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (isEscaping)
        {
            agent.speed = escapeSpeed;
            float timeTaken = TimeManager.instance.inGameTime - escapeStart;
            // Calculate direction from murderer to player
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            // Calculate escape point in opposite direction
            Vector3 escapePoint = transform.position - directionToPlayer * escapeRange;
            agent.SetDestination(escapePoint);
            if (Vector3.Distance(transform.position, escapePoint) <= 0.5f || timeTaken > 3)
            {
                deactivate();
            }
        }
        else
        {
            agent.speed = killSpeed;
            agent.SetDestination(target.position);
            if (distanceToTarget <= killRange)
            {
                Debug.Log("Murderer has killed the player");
                // Freeze the player
                playerController.isDead = true;
            }
        }
    }

    private void updateSpeed()
    {
        float time = TimeManager.instance.inGameTime;
        // Each full day is 210 seconds = 90 + 90 + 30
        // Each day that passes, kill speed goes up by 0.75 until it caps out at 6
        int daysPassed = (int)(time / 210);
        killSpeed = Mathf.Min(6, baseSpeed + 0.75f * daysPassed);
    }


    public void activate()
    {
        transform.position = nightSpawnPoint;
        isEscaping = false;
        gameObject.SetActive(true);
    }

    public void deactivate()
    {
        gameObject.SetActive(false);
    }

    public void escape()
    {
        if (!isEscaping)
        {
            isEscaping = true;
            escapeStart = TimeManager.instance.inGameTime;
        }
    }

    public bool isActive()
    {
        return gameObject.activeSelf;
    }
}
