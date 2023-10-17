using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LairEnemy : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<FieldOfView>().CanSeePlayer)
        {
            transform.GetComponent<PatrolNpc>().enabled = false;
            transform.GetComponent<MurdererController>().enabled = true;
        }
        else
        {
            transform.GetComponent<PatrolNpc>().enabled = true;
            transform.GetComponent<MurdererController>().enabled = false;
        }
    }
}