using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceBox : MonoBehaviour
{
    public GameObject prompt;
    public DialogManager dialogManager;
    public SceneInfo playerStorage;
    public GameObject submissionImage;

    private bool inRange;
    private bool dialogShown = false;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            if (CollectedAllClues())
            {
                submissionImage.SetActive(true);
            }
            else if (dialogShown)
            {
                dialogManager.HideDialog();
                dialogShown = false;
            }
            else
            {
                dialogManager.ShowDialog();
                dialogManager.SetDialog("Collect all the clues before submitting your report.");
                dialogManager.SetLabel("Evidence");
                dialogShown = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player") )
        {
            inRange = true;
            prompt.SetActive(true);
        }  
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            inRange = false;
            prompt.SetActive(false);
        }
    }

    private bool CollectedAllClues()
    {
        foreach (bool clue in playerStorage.cluesFound)
        {
            if (!clue)
            {
                return false;
            }
        }
        return true;
    }
}
