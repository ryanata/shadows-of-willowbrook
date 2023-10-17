using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfView : MonoBehaviour
{

    public float viewRadius = 5f;
    [Range(1, 360)] public float viewAngle = 45f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    public GameObject playerRef;
    public bool CanSeePlayer { get; private set; }

    NavMeshAgent agent;

    public Vector3 ForwardDir { get; private set; }
    private float angle01 = 0;
    private float angle02 = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(FOVCheck());
    }

    private IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FOV();
        }
    }

    private void FOV()
    {
        if (!agent.pathPending && agent.remainingDistance > 0.5f)
        {
            ForwardDir = (agent.steeringTarget - transform.position).normalized;
            angle01 = AngleFromDir(ForwardDir, -viewAngle / 2);
            angle02 = AngleFromDir(ForwardDir, viewAngle / 2);
        }

        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetLayer);

        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (IsWithinFieldOfView(directionToTarget) && !IsObstructed(directionToTarget, target))
                CanSeePlayer = true;
            else
                CanSeePlayer = false;
        }
        else if (CanSeePlayer)
            CanSeePlayer = false;
    }

    private bool IsWithinFieldOfView(Vector2 directionToTarget)
    {
        float angleToTarget = AngleFromDir(directionToTarget, 0);
        return angleToTarget >= angle01 && angleToTarget <= angle02;
    }

    private bool IsObstructed(Vector2 directionToTarget, Transform target)
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        return Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer);
    }

    private float AngleFromDir(Vector3 direction, float offsetAngleInDegrees)
    {
        float angleInRadians = Mathf.Atan2(direction.y, direction.x);
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg + offsetAngleInDegrees;
        return angleInDegrees;
    }

    private Vector3 DirFromAngle(float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, viewRadius);

        if (agent)
        {
            Vector3 targetDir = (agent.steeringTarget - transform.position).normalized;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + DirFromAngle(angle01) * viewRadius);
            Gizmos.DrawLine(transform.position, transform.position + DirFromAngle(angle02) * viewRadius);
        }

        if (CanSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, playerRef.transform.position);
        }
    }
}