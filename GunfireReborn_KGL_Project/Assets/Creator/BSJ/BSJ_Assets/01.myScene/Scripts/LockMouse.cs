using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMouse : MonoBehaviour
{
    private bool isMouseLocked = true; // 마우스 잠금 상태를 나타내는 변수

    private void Start()
    {
        LockMouseCursor();
    }

    private void Update()
    {
        // ESC 키를 누르면 마우스 잠금 상태를 토글합니다.
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isMouseLocked = !isMouseLocked;
            LockMouseCursor();
        }
    }

    private void LockMouseCursor()
    {
        if (isMouseLocked)
        {
            Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
            Cursor.visible = false; // 마우스 커서 숨김
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; // 마우스 잠금 해제
            Cursor.visible = true; // 마우스 커서 표시
        }
    }
}
