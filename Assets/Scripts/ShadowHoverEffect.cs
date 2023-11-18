using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShadowHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject shadow;

    public void OnPointerEnter(PointerEventData eventData)
    {
        shadow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shadow.SetActive(false);
    }
}
