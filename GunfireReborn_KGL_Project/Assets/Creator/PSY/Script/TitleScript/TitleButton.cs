using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler , IPointerClickHandler
{
    private Image bt; // 버튼 이미지
    public string buttonType;  // 버튼 타입

    private GameObject popupExit;  // 종료 확인 팝업

    private void Start()
    {
        bt = GetComponent<Image>();  // 버튼 이미지의 컴포넌트를 가져온다.

        bt.fillAmount = 0;  // 시작할 때 버튼의 배경을 0으로 한다.

        popupExit = GameObject.Find("PopupExit");  // 종료 확인 팝업을 가져와서 변수에 넣는다.
        popupExit?.SetActive(false);  // 비활성화 시킨다.
    }

    public void OnPointerEnter(PointerEventData eventData)  // 마우스가 해당 범위 위에 있다면 해당 함수를 실행
    {
        StartCoroutine(OpenButton());  // 코루틴 함수 실행
    }

    public void OnPointerExit(PointerEventData eventData)  // 마우스가 해당 범위에서 벗어 났다면 해당 함수를 실행
    {
        StopAllCoroutines();  // 모든 코루틴을 종료시킨다.
        bt.fillAmount = 0;    // 버튼의 배경을 0으로 한다.
    }  

    IEnumerator OpenButton()  // 코루틴 : 일정 딜레이 값을 주고 싶을 때 쓰는 함수
    {
        while ( bt.fillAmount < 1 )  // 버튼의 배경이 1보다 작을 때까지 실행
        {
            bt.fillAmount += 0.02f;  // 버튼의 이미지에 해당 값만큼을 더해간다.
            yield return new WaitForSeconds(Time.deltaTime);  // 반복하는 딜레이를 Time.deltaTime 만큼을 준다.
        }

        yield break;  // 위의 반복문을 벗어나면 해당 코루틴 종료 ( 필수 )
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ( buttonType == "종료" )
        {
            popupExit?.SetActive(true);  // 종료 확인 팝업을 활성화 시킨다.
        } 
        else
        {
            SceneManager.LoadScene("LoadingScene_PSY");
        }
    }
}
