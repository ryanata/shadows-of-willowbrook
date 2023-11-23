using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueController : MonoBehaviour {
    public GameObject prompt;
    public int clueNumber;
    public SceneInfo playerStorage;
    private PlayerController playerController;
    private bool pickUpAllowed;

    // Start is called before the first frame update
    private void Start () 
    {
        if (playerStorage.cluesFound[clueNumber-1])
        {
            Destroy(gameObject);
        }
        playerController = FindObjectOfType<PlayerController>();
    }
	// Update is called once per frame
	private void Update () 
    {
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E) && !playerController.isInDialog)
        {
            this.PickUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            pickUpAllowed = true;
            prompt.SetActive(true);
        }  
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            pickUpAllowed = false;
            prompt.SetActive(false);
        }
    }

    private void PickUp()
    {
        // Save the clue in global store
        playerStorage.cluesFound[clueNumber-1] = true;
        Destroy(gameObject);
    }

}
