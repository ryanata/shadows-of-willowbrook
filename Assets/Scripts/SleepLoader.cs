using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SleepLoader : MonoBehaviour
{
    private PlayerController playerController;
    public Animator transition;
    private float timeInCycle;
    public float transitionTime = 10;
    public SceneInfo playerStorage;
    // Update is called once per frame

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        if (playerController.isInBed == true /*&& IsTime(TimeOfDay.Night)*/)
        {
            Debug.Log("Is in bed");
            LoadNextLevel();
            playerController.isInBed = false;
        }
        timeInCycle = TimeManager.instance.inGameTime % (playerStorage.dayDuration + playerStorage.transitionDuration + playerStorage.nightDuration + playerStorage.transitionDuration);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadStart());
        //StartCoroutine(LoadEnd());
    }

    IEnumerator LoadStart()
    {
        Debug.Log("LoadStart called");
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        Debug.Log("LoadStart completed");
        StartCoroutine(LoadEnd());
    }

    IEnumerator LoadEnd()
    {
        Debug.Log("LoadEnd called");
        transition.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);
        Debug.Log("LoadEnd completed");
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