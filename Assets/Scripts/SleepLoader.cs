using System;
using System.Collections;
using System.Collections.Generic;
// using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SleepLoader : MonoBehaviour
{
    public BedInteractor bed;
    public Animator transition;
    public float transitionTime = 5;
    public SceneInfo playerStorage;
    
    private float timeInCycle;
    private PlayerController playerController;
    private bool sleeping = false;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        timeInCycle = TimeManager.instance.inGameTime % (playerStorage.dayDuration + playerStorage.transitionDuration + playerStorage.nightDuration + playerStorage.transitionDuration);

        if (bed.clickedBed && IsTime(TimeOfDay.Night) && !sleeping)
        {
            LoadNextLevel();
            sleeping = true;
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadStart());
        //StartCoroutine(LoadEnd());
    }

    IEnumerator LoadStart()
    {
        transition.SetTrigger("Start");
        playerController.isInDialog = true; // Freeze player
        yield return new WaitForSeconds(transitionTime);
        playerController.isInDialog = false; // Unfreeze player

        // Change time to morning
        if (IsTime(TimeOfDay.Night))
        {
            int cycle = (int)(playerStorage.dayDuration + 2*playerStorage.transitionDuration + playerStorage.nightDuration);
            int timeInCycle = (int)TimeManager.instance.inGameTime % cycle;
            int timeUntilMorning = (cycle - (int)playerStorage.transitionDuration) - timeInCycle;
            if (timeUntilMorning > 0)
            {
                Debug.Log("Adding " + timeUntilMorning + " to time");
                TimeManager.instance.inGameTime += timeUntilMorning;
            }
        }
        sleeping = false;
        StartCoroutine(LoadEnd());
    }

    IEnumerator LoadEnd()
    {
        transition.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);
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