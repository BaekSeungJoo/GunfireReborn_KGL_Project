using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    private GameObject popupExit;

    private void Start()
    {
        popupExit = GameObject.Find("PopupExit");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            popupExit.SetActive(true);
        }
    }
}
