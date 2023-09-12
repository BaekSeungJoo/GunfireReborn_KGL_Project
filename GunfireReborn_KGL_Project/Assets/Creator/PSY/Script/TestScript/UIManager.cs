using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject shopPopup;

    private void Start()
    {
        shopPopup = GameObject.Find("ShopPopupCanvas");
    }

    /// <summary>
    /// ShopPopup 활성화 세팅 함수
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActiveShopPopup( bool isActive )
    {
        shopPopup?.SetActive( isActive ); 
    }

    /// <summary>
    /// ShopPopup 활성화 체크 함수 
    /// </summary>
    public bool CheckActiveShopPopup()
    {
        if ( shopPopup.activeInHierarchy )
        {
            return true;
        }
        return false;
    }
}
