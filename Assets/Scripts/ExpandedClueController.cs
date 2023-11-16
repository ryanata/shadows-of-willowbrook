using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandedClueController : MonoBehaviour
{
    public GameObject journal;
    // Update is called once per frame
    void Update()
    {
        // If the player presses escape, then hide the clue information image
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            journal.SetActive(true);
        }
    }
}
