using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseToggle : MonoBehaviour
{
    public void activate(bool status)
    {
        gameObject.SetActive(status);
    }
}
