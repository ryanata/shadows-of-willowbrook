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

    public float transitionTime = 10;
    // Update is called once per frame

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        if(playerController.isInBed == true && playerController.canSleep == true)
        {
            Debug.Log("Is in bed");
            LoadNextLevel();
            playerController.isInBed = false;
        }
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
}
