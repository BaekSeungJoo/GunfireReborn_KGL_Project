using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject shopPopup;        // 상점 팝업
    private ShopManager shopManager;

    private TextMeshProUGUI text_Reroll; // 새로고침 텍스트

    #region Exit 변수
    private Button bt_Exit;              // ESC 버튼
    private TextMeshProUGUI text_Exit;   // ESC 텍스트
    #endregion

    private void Start()
    {
        shopPopup = GameObject.Find("ShopPopupCanvas");
        shopManager = GameObject.Find("@Managers").GetComponent<ShopManager>();

        text_Reroll = GameObject.Find("RerollText").GetComponent<TextMeshProUGUI>();
        
        bt_Exit = GameObject.Find("ExitButton").GetComponent<Button>();
        text_Exit = GameObject.Find("ExitText").GetComponent <TextMeshProUGUI>();
    }

    /// <summary>
    /// ShopPopup 활성화 체크 함수 
    /// </summary>
    public bool CheckActiveShopPopup()
    {
        if (shopPopup.activeInHierarchy)
        {
            return true;
        }
        return false;
    }

    #region 이벤트 함수
    /// <summary>
    /// ExitButton 이벤트 함수
    /// </summary>
    public void OnExitButton()
    {  // ESC 버튼 클릭 시 상점 팝업을 닫는다.
        shopPopup.SetActive(false);
    }

    /// <summary>
    /// RerollButton 이벤트 함수
    /// </summary>
    public void OnRerollButton()
    {  // 새로고침 버튼 클릭 시 새로고침 횟수가 차감되는 함수 실행
        SetRerollText();
    }
    #endregion

    #region 세팅 함수
    /// <summary>
    /// RerollText 세팅 함수
    /// </summary>
    /// <param name="count">새로고침 횟수</param>
    public void SetRerollText()
    {  // 새로고침 버튼 클릭 시 클릭 횟수가 차감된 텍스트 출력 함수
        if ( shopManager.Count > 0 )
        {
            shopManager.Count--;
            text_Reroll.text = $"새로 고침 <i> {shopManager.Count} / 3 <i> ";
        }
    }

    /// <summary>
    /// ShopPopup 활성화 세팅 함수
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActiveShopPopup(bool isActive)
    {  // 상점 팝업 활성화 / 비활성화 함수
        shopPopup?.SetActive(isActive);
    }
    #endregion
}
