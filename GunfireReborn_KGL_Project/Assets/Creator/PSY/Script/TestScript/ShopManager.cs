using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private UIManager uiManager;

    private void Start()
    {
        uiManager = GameObject.Find("@Managers").GetComponent<UIManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && uiManager.CheckActiveShopPopup() )
        {
            uiManager.SetActiveShopPopup(false);

        }    
    }
}
