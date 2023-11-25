using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGameController : MonoBehaviour
{
    public SceneInfo playerStorage;
    public DialogManager dialogManager;

    // Start is called before the first frame update
    void Start()
    {
        if (playerStorage.guessedRight)
        {
            dialogManager.ShowDialog();
            dialogManager.SetDialog("You cracked the case! Lillian killed Evelyn with a toxic potion after being confronted about her flower cult!");
            dialogManager.SetLabel("Police");
        }
        else
        {
            dialogManager.ShowDialog();
            dialogManager.SetDialog("Sorry detective, we got the wrong person. Maybe another time.");
            dialogManager.SetLabel("Police");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
