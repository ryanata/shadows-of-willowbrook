using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowClue : MonoBehaviour
{
    public GameObject clueImage;
    public GameObject questionMarkImage;
    public int clueNumber;
    public SceneInfo playerStorage;

    private bool foundClue = false;
    // Start is called before the first frame update
    void Start()
    {
        clueImage.SetActive(false);
        questionMarkImage.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // If clueNumber is in playerStorage.cluesFound, then
        // we should show the clueImage and hide the hiddenImage.
        // Otherwise, we should hide the clueImage and show the hiddenImage.
        if (playerStorage.cluesFound[clueNumber - 1] && !foundClue)
        {
            clueImage.SetActive(true);
            questionMarkImage.SetActive(false);
            foundClue = true;
        }
    }
}
