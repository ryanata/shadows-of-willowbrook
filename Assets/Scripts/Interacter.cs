using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{
    public GameObject prompt;
    public SceneInfo playerStorage;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isInDialog || playerStorage.dialogueRead[0].baseDialogue)
        {
            prompt.SetActive(false);
        }
    }
}
