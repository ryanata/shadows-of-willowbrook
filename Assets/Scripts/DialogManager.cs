using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [TextArea(10, 20)] [SerializeField] private string content;
    [Space] [SerializeField] private TMP_Text textComponent;

    private void Awake()
    {
        textComponent.text = content;
    }

    public void SetDialog(string dialog)
    {
        textComponent.pageToDisplay = 1;
        textComponent.text = dialog;
    }

    public void ShowDialog()
    {
        gameObject.SetActive(true);
    }

    public void HideDialog()
    {
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void ContinueDialog()
    {
        textComponent.pageToDisplay++;
    }

    public bool HasReachedEnd()
    {
        return textComponent.pageToDisplay == textComponent.textInfo.pageCount;
    }
}

