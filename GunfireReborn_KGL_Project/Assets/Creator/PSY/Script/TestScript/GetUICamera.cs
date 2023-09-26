using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetUICamera : MonoBehaviour
{
    // UI 카메라를 저장할 변수
    private Camera uiCamera;
    private Canvas minimapCanvas;

    private void Awake()
    {
        // Canvas 컴포넌트 찾기
        minimapCanvas = GetComponent<Canvas>();
    }
 
    void Start()
    {
        // 하이어라키 창에서 UI 카메라를 찾음
        uiCamera = GameObject.Find("UI Camera").GetComponent<Camera>();

        // UI 카메라가 있을 경우, 해당 카메라의 컴포넌트나 설정을 사용할 수 있음
        if (uiCamera != null)
        {
            // Canvas 컴포넌트의 이벤트 카메라에 UI 카메라 할당
            minimapCanvas.worldCamera = uiCamera;
        }
    }
}
