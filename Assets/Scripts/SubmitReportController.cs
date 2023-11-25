using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SubmitReportController : MonoBehaviour
{
    public string selected;
    public SceneInfo playerStorage;
    // TMPButton
    public Button button;

    // Update is called once per frame
    void Update()
    {
        if (selected != null)
        {
            // Make button clickable
            button.interactable = true;
        }

        // If user press escape, close object
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    public void SubmitReport()
    {
        playerStorage.guessedRight = selected == "Lillian";
        SceneManager.LoadScene("PostGameScene");
    }

}
