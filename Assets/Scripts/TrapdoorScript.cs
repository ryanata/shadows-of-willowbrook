using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapdoorScript : MonoBehaviour
{
    public GameObject prompt;
    public DialogManager dialogManager;
    public SceneInfo playerStorage;
    public Vector2 startPosition;

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
            if (playerStorage.cluesFound[7])
            {
                playerStorage.startPosition = startPosition;
                SceneManager.LoadSceneAsync("SecretLayerScene");
            }
            else if (dialogShown)
            {
                dialogManager.HideDialog();
                dialogShown = false;
                playerController.isInDialog = false;
            }
            else
            {
                dialogManager.ShowDialog();
                dialogManager.SetDialog("It looks like I need a key to access this door. Let's find it without looking suspicious...");
                dialogManager.SetLabel("Detective");
                dialogShown = true;
                playerController.isInDialog = true;
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

}
