using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class BedInteractor : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.E;
    private PlayerController playerController;
    private bool isPlayerInRange = false;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            // Debug
            Debug.Log("Player pressed interaction key");
            playerController.isInBed = true;
            
        }
    }
}

