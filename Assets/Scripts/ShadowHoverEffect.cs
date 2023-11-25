using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShadowHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject shadow;
    public SubmitReportController parentController;
    public string suspectName;

    private bool setSuspect = false;
    private bool isHovering = false;
    private bool isClicked = false;

    // Update is called once per frame
    void Update()
    {
        if (parentController.selected == suspectName)
        {
            isClicked = true;
            shadow.SetActive(true);
        }
        else
        {
            if (shadow.activeSelf && isClicked && !isHovering)
            {
                shadow.SetActive(false);
                isClicked = false;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        parentController.selected = suspectName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        shadow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (!isClicked)
        {
            shadow.SetActive(false);
        }
    }
}
