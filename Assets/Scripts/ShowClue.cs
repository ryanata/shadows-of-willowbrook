using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowClue : MonoBehaviour, IPointerClickHandler
{
    public GameObject journal;
    public GameObject clueImage;
    public GameObject questionMarkImage;
    public GameObject clueInformation;
    public int clueNumber;
    public SceneInfo playerStorage;

    private bool foundClue = false;
    // Start is called before the first frame update
    void Start()
    {
        clueImage.SetActive(false);
        questionMarkImage.SetActive(true);
        clueInformation.SetActive(false);
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

    public void OnPointerClick(PointerEventData eventData)
    {
        // If the clue has been found, show the clue information image
        if (foundClue)
        {
            clueInformation.SetActive(true);
            journal.SetActive(false);
        }
    }
}
