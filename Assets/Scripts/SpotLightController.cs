using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightController : MonoBehaviour
{

    public GameObject attachedNpc;

    // Update is called once per frame
    void Update()
    {
        if (attachedNpc != null)
        {
            transform.position = attachedNpc.transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, attachedNpc.GetComponent<FieldOfView>().ForwardDir);
        }
    }
}