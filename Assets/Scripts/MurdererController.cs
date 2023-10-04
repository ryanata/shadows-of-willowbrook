using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MurdererController : MonoBehaviour
{
    public PlayerController playerController;
    public float killRange = 3f;
    public bool hasKilledPlayer = false;

    private NavMeshAgent agent;
    private Transform target;

    private void Start()
    {
        target = playerController.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (hasKilledPlayer) return;
        agent.SetDestination(target.position);
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= killRange)
        {
            Debug.Log("Murderer has killed the player");
            // Freeze the player; HACK: if player is in dialog, they're frozen
            playerController.isInDialog = true;
            // Freeze the murderer
            agent.enabled = false;

            hasKilledPlayer = true;
        }
    }
}
