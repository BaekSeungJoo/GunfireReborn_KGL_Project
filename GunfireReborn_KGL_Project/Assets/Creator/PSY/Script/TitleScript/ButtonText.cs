using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonText : MonoBehaviour ,IPointerEnterHandler , IPointerExitHandler
{
    public TextMeshProUGUI Text; 
    private Color baseColor;    // 기본 컬러
    private Color changeColor;  // 바뀌는 컬러

    private void Start()
    {
        baseColor = Text.color;  // 기본 컬러를 지정
    }

    #region 이벤트 함수
    /// <summary>
    /// 마우스가 해당 범위 위에 있다면 실행되는 이벤트 함수
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)  // 마우스가 해당 범위 위에 있다면 해당 함수를 실행
    {
        ColorUtility.TryParseHtmlString("#C29024FF", out changeColor);  // 바뀔 컬러를 changeColor에 저장
        Text.color = changeColor;  // 바뀌는 컬러는 현재 text.color 에 대입한다.
        Text.fontSize = 60;  // 현재 폰트 사이즈를 60으로 변경
    }

    /// <summary>
    /// 마우스가 해당 범위애서 벗어 났다면 실행되는 이벤트 함수
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)  // 마우스가 해당 범위에서 벗어 났다면 해당 함수를 실행
    {
        Text.color = baseColor;  // 기본 컬러를 현재 text.color 에 대입한다.
        Text.fontSize = 50;      // 현재 폰트 사이즈를 50으로 변경
    }
    #endregion
}
