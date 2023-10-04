using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public TMP_Text dialogText;

    private void Awake()
    {
        dialogText = GetComponentInChildren<TMP_Text>();
    }

    public void ShowDialog()
    {
        gameObject.SetActive(true);
        Debug.Log("Dialog shown");
    }

    public void HideDialog()
    {
        gameObject.SetActive(false);
    }

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }
}

