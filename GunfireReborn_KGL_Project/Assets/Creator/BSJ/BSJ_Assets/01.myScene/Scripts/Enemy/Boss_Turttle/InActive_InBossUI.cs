using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InActive_InBossUI : MonoBehaviour
{
    private Canvas mainUiCanvas;

    private void Start()
    {
        mainUiCanvas = GameObject.Find("MainUICanvas").GetComponent<Canvas>();

        mainUiCanvas?.transform.GetChild(0).gameObject.SetActive(false);
        mainUiCanvas?.transform.GetChild(1).gameObject.SetActive(false);
        mainUiCanvas?.transform.GetChild(2).gameObject.SetActive(false);
        
    }
}
