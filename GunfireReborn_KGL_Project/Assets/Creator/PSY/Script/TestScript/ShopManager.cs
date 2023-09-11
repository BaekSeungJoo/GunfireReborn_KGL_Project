using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPopup;

    private void Start()
    {
        shopPopup = GameObject.Find("ShopPopupFrame");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && shopPopup.activeInHierarchy )
        {
            shopPopup.SetActive(false);
        }    
    }
}
