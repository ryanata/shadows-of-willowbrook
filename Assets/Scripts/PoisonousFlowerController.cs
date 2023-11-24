using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousFlowerController : MonoBehaviour
{
    public SceneInfo playerStorage;
    public GameObject interactPrompt;

    private ClueController clueController;
    private float timeInCycle;
    // Start is called before the first frame update
    void Start()
    {
        // Find script called "ClueController" on the same object
        clueController = GetComponent<ClueController>();
    }

    // Update is called once per frame
    void Update()
    {
        timeInCycle = TimeManager.instance.inGameTime % (playerStorage.dayDuration + playerStorage.transitionDuration + playerStorage.nightDuration + playerStorage.transitionDuration);
        
        if (IsTime(TimeOfDay.Night))
        {
            // Enable clueController
            clueController.enabled = true;
        }
        else
        {
            clueController.enabled = false;
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
