using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Exit Popup Class
public class CheckExitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler , IPointerClickHandler
{
    public GameObject popupExit;

    public Image bt;

    private Color baseColor;   // 기본 컬러
    private Color changeColor; // 바뀌는 컬러

    public string checkType;   // 버튼 타입

    private void Start()
    {
        popupExit = GameObject.Find("PopupExit");

        bt = GetComponent<Image>();
        baseColor = bt.color;  // 기본 컬러를 저장한다.
    }

    #region 이벤트 함수
    /// <summary>
    /// 마우스가 해당 범위 위에 있다면 실행되는 이벤트 함수
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    { 
        ColorUtility.TryParseHtmlString("#FFFFFFF", out changeColor);
        bt.color = changeColor;  // 컬러를 바꾸고 바꾼 컬러를 저장한다.
    }

    /// <summary>
    /// 마우스가 해당 범위애서 벗어 났다면 실행되는 이벤트 함수
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        bt.color = baseColor;  // 기본 컬러로 다시 변경
    }

    /// <summary>
    /// 마우스가 해당 범위를 클릭하면 실행되는 이벤트 함수
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if ( checkType == "Yes" )  // 버튼 타입이 "Yes" 라면 if문 실행
        {
            Application.Quit();  // 게임을 종료한다,
        }
        else
        {
            popupExit?.SetActive(false);  // 버튼 타입이 "Yes"가 아니라면 종료 확인 팝업을 닫는다.
        }
    }
    #endregion
}
