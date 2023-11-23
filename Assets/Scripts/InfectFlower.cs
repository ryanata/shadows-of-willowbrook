using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectFlower : MonoBehaviour
{
    public float infectionTime;
    // Infected sprite
    public Sprite infectedSprite;

    private bool infected = false;
    // Update is called once per frame
    void Update()
    {
        if (!infected && infectionTime <= TimeManager.instance.inGameTime)
        {
            GetComponent<SpriteRenderer>().sprite = infectedSprite;
            infected = true;
        }
    }
}
