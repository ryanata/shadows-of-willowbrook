using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvasiveFlower : MonoBehaviour
{
    public int fot;
    public Sprite InvFlower;

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = InvFlower;
    }
}

