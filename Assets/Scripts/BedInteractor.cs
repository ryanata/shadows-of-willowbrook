using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class BedInteractor : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.E;
    public bool clickedBed = false;
    public DialogManager dialogManager;
    public SceneInfo playerStorage;
    public GameObject interactPrompt;
    private float timeInCycle;
    private PlayerController playerController;
    private bool isPlayerInRange = false;

    private void Start()
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
        timeInCycle = TimeManager.instance.inGameTime % (playerStorage.dayDuration + playerStorage.transitionDuration + playerStorage.nightDuration + playerStorage.transitionDuration);

        if (isPlayerInRange)
        {
            interactPrompt.SetActive(true);
            if (Input.GetKeyDown(interactionKey))
            {
                if (IsTime(TimeOfDay.Night))
                {
                    dialogManager.HideDialog();
                    playerController.isInDialog = false;
                    clickedBed = true;
                }
                else
                {
                    if (dialogManager.IsActive())
                    {
                        dialogManager.HideDialog();
                        playerController.isInDialog = false;
                        return;
                    }
                    dialogManager.ShowDialog();
                    dialogManager.SetDialog("I can't sleep now. It's not even night time yet.");
                    dialogManager.SetLabel("Detective");
                    playerController.isInDialog = true;
                }
            }
        }
        else
        {
            interactPrompt.SetActive(false);
        }
    }

    enum TimeOfDay
    {
        Day,       // 0
        Evening,   // 1
        Night,     // 2
        Morning    // 3
    }

    bool IsTime(TimeOfDay marker)
    {
        TimeOfDay mark = TimeOfDay.Day;
        if (timeInCycle < playerStorage.dayDuration)
        {
            mark = TimeOfDay.Day;
        }
        else if (timeInCycle < playerStorage.dayDuration + playerStorage.transitionDuration)
        {
            mark = TimeOfDay.Evening;
        }
        else if (timeInCycle < playerStorage.dayDuration + playerStorage.transitionDuration + playerStorage.nightDuration)
        {
            mark = TimeOfDay.Night;
        }
        else
        {
            mark = TimeOfDay.Morning;
        }

        return mark == marker;
    }
}