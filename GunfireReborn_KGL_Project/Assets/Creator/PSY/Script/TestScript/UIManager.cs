using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region 상점
    private GameObject shopPopup;        // 상점 팝업
    private ShopManager shopManager;
    #endregion

    private GameObject invenCanvas;      // 가방 UI

    private GameObject blackSmithCanvas; // 대장장이 UI

    private GameObject mainCanvas;       // 메인 UI

    private TextMeshProUGUI text_Reroll; // 새로고침 텍스트

    private GameObject toolTip;          // 인벤토리 툴팁
    private TextMeshProUGUI toolTipTitleText;
    private TextMeshProUGUI toolTipInfoText;

    #region Exit 변수
    private Button bt_Exit;              // ESC 버튼
    private TextMeshProUGUI text_Exit;   // ESC 텍스트
    #endregion

    private void Start()
    {
        shopPopup = GameObject.Find("ShopPopupCanvas");
        shopManager = GameObject.Find("@Managers").GetComponent<ShopManager>();

        invenCanvas = GameObject.Find("InventoryCanvas");

        blackSmithCanvas = GameObject.Find("BlackSmithCanvas");

        mainCanvas = GameObject.Find("MainCanvas");

        text_Reroll = GameObject.Find("RerollText").GetComponent<TextMeshProUGUI>();
        
        bt_Exit = GameObject.Find("ExitButton").GetComponent<Button>();
        text_Exit = GameObject.Find("ExitText").GetComponent <TextMeshProUGUI>();

        toolTip = GameObject.Find("ToolTip");

        toolTipTitleText = toolTip.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        toolTipInfoText = toolTip.transform.Find("ItemInfo").GetComponent<TextMeshProUGUI>();



        SetActiveMainCanvas(true);
        SetActiveInven(false);
        //SetActiveShopPopup(false);
        SetActiveBlackSmith(false);
        SetActiveToolTip(false);

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
    {  // ESC 버튼 클릭 시 상점 / 가방 팝업을 닫는다.
        shopPopup.SetActive(false);
        invenCanvas.SetActive(false);
        blackSmithCanvas.SetActive(false);
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
        SetActiveMainCanvas(!isActive);  // 상점 팝업과 반대로 세팅
    }

    /// <summary>
    /// BlackSmith 활성화 세팅 함수
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActiveBlackSmith(bool isActive)
    {  // 대장장이 UI 활성화 / 비활성화 함수
        blackSmithCanvas?.SetActive(isActive);
        SetActiveMainCanvas(!isActive);  // 대장장이 팝업과 반대로 세팅
    }

    /// <summary>
    /// Inventory 활성화 세팅 함수
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActiveInven(bool isActive)
    {  // 가방 UI 활성화 / 비활성화 함수
        invenCanvas?.SetActive(isActive);
        SetActiveMainCanvas(!isActive);  // 가방 팝업과 반대로 세팅
    }

    /// <summary>
    /// MainCanvas 활성화 세팅 함수
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActiveMainCanvas(bool isActive)
    {  // 메인 UI 활성화 / 비활성화 함수
        mainCanvas?.SetActive(isActive);
    }

    /// <summary>
    /// ToolTip 활성화 세팅 함수
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActiveToolTip(bool isActive)
    {
        toolTip.SetActive(isActive);
    }

    /// <summary>
    /// ToolTip 활성화 정보 세팅 함수
    /// </summary>
    /// <param name="itemData">활성화 할 아이템</param>
    public void SetToolTipText(ItemData itemData)
    {
        toolTipTitleText.text = itemData.name;
        toolTipInfoText.text = itemData.info;
    }
    #endregion


}
