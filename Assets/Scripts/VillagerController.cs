using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    // This is the key that the player will press to interact with the villager
    public KeyCode interactionKey = KeyCode.E;
    public DialogManager dialogManager;

    [TextArea(3, 10)]
    public string[] dialogLines;


    [TextArea(3, 10)]
    public string[] afterDialogLine;

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
        if (isPlayerInRange && Input.GetKeyDown(interactionKey) && (currentLine < dialogLines.Length))
        {
            // Debug
            Debug.Log("Player pressed interaction key");
            playerController.isInDialog = true;
            dialogManager.ShowDialog();
            this.NextLine();
        }
        else if(isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            if (playerController.isInDialog == false)
            {
                Debug.Log("Player false");
                dialogManager.ShowDialog();
                var r = new System.Random();
                var rNum = r.Next(0, afterDialogLine.Length);
                dialogManager.SetDialog(afterDialogLine[rNum]);
                playerController.isInDialog = true;
            }
            else
            {
                Debug.Log("Player true");
                dialogManager.HideDialog();
                playerController.isInDialog = false;
            }
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
