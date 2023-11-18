using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    public string villagerName;
    public KeyCode interactionKey = KeyCode.E;
    public DialogManager dialogManager;
    public SceneInfo playerStorage;

    private Queue<string> dialogLines = new Queue<string>();
    // Bool array to keep track of which dialogue have been added
    private bool[] dialogueAdded = new bool[8];
    private PlayerController playerController;
    private bool isPlayerInRange = false;
    private bool isConversationDry = true;


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
        AddDialog();
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            if (dialogLines.Count > 0)
            {
                isConversationDry = false;
            }
            this.NextLine();
        }
    }

    private void NextLine()
    {
        if (dialogManager.IsActive() && !dialogManager.HasReachedEnd())
        {
            dialogManager.ContinueDialog();
            return;
        }
        // If dialogLines is empty, hide the dialog box)
        if (dialogLines.Count == 0)
        {
            if (isConversationDry)
            {
                // Set the dialogue to a random dry dialogue
                dialogManager.ShowDialog();
                SetDialogAndLabel("Villager: I don't have anything to say to you.");
                playerController.isInDialog = true;
                isConversationDry = false;
                return;
            }
            dialogManager.HideDialog();
            playerController.isInDialog = false;
            isConversationDry = true;
            return;
        }
        string dialog = dialogLines.Dequeue();
        if (dialog.StartsWith("System: "))
        {
            SystemPrompt(dialog);
            NextLine();
            return;
        }
        dialogManager.ShowDialog();
        playerController.isInDialog = true;
        SetDialogAndLabel(dialog);
    }

    private void SystemPrompt(string prompt)
    {
        // If prompt starts with "System: ", then it's a system prompt
        // System prompts are everything after "System: " and it indicates which playerStorage.dialogueRead[] to update
        string systemPrompt = prompt.Substring(8);
        switch (villagerName)
        {
            case "Police":
                switch (systemPrompt)
                {
                    case "base":
                        playerStorage.dialogueRead[0].baseDialogue = true;
                        break;
                }
                break;
            case "Mayor":
            case "Samuel":
            case "Isabel":
            case "Lillian":
            case "Walter":
            default:
                break;
            
        }
    }

    // Create a help function with a decision tree to
    // determine which dialog to use based on villager, time, and more conditions
    private void AddDialog()
    {
        switch (villagerName)
        {
            case "Police":
                if (!dialogueAdded[0] && !playerStorage.dialogueRead[0].baseDialogue)
                {
                    dialogLines.Enqueue("Villager: Detective, thank goodness you're here! We've called upon you because you're one of the best investigators around Willowbrook. Quite frankly, you're our last hope in solving this crime.");
                    dialogLines.Enqueue("Detective: What's happened here officer?");
                    dialogLines.Enqueue("Villager: Early this morning, Evelyn Greenfield, the village florist, was found dead in the forest. It's a small village and we've never had a case like this before!");
                    dialogLines.Enqueue("Detective: I see. How much do we already know officer?");
                    dialogLines.Enqueue("Villager: Not much I'm afraid. Explore the village, gather clues, and talk to the villagers. We need 8 clues total before we can prosecute");
                    dialogLines.Enqueue("Villager: You should have a journal, with all your clues, that you can see by pressing J. Go to my office to collect your first clue. It's a detailed book I've prepared for you with all the information we have so far.");
                    dialogLines.Enqueue("Detective: Thank you officer. I'll get to work right away.");
                    dialogLines.Enqueue("Villager: Good luck, Detective. Willowbrook is counting on you to bring justice for Evelyn!");
                    dialogLines.Enqueue("System: base");
                    dialogueAdded[0] = true;
                }
                break;
            case "Mayor":
            case "Samuel":
            case "Isabel":
            case "Lillian":
            case "Walter":
            default:
                break;
        }
    }

    private void SetDialogAndLabel(string text)
    {
        // Grab everything before ": " and set it as the label
        // Grab everything after ": " and set it as the dialog
        string[] splitText = text.Split(new string[] { ": " }, System.StringSplitOptions.None);
        dialogManager.SetDialog(splitText[1]);
        if (splitText[0] == "Villager")
        {
            dialogManager.SetLabel(villagerName);
        }
        else
        {
            dialogManager.SetLabel(splitText[0]);
        }
    }
}
