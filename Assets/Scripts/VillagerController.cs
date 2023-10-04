using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    // This is the key that the player will press to interact with the villager
    public KeyCode interactionKey = KeyCode.E;
    public DialogManager dialogManager;
    public string[] dialogLines;

    private PlayerController playerController;
    private int currentLine = 0;
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
            playerController.isInDialog = true;
            dialogManager.ShowDialog();
            this.NextLine();
        }
    }

    private void NextLine()
    {
        if (currentLine < dialogLines.Length)
        {
            dialogManager.SetDialog(dialogLines[currentLine]);
            currentLine++;
        }
        else
        {
            dialogManager.HideDialog();
            playerController.isInDialog = false;
        }
    }
}
