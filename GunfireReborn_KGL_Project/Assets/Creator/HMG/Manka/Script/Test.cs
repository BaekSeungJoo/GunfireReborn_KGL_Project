using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class MouseLook : MonoBehaviour
{
    public static MouseLook instance;

    [Header("설정")]
    public Vector2 clampInDegrees = new Vector2(360, 180);
    public bool lockCursor = true;
    [Space]
    private Vector2 sensitivity = new Vector2(2, 2);
    [Space]
    public Vector2 smoothing = new Vector2(3, 3);

    [Header("일인칭 시점")]
    public GameObject characterBody;

    private Vector2 targetDirection;
    private Vector2 targetCharacterDirection;

    private Vector2 _mouseAbsolute;
    private Vector2 _smoothMouse;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool scoped;

    void Start()
    {
        
        instance = this;

        // 카메라의 초기 방향으로 목표 방향 설정
        targetDirection = transform.localRotation.eulerAngles;

        // 캐릭터 본체의 초기 상태로 목표 방향 설정
        if (characterBody)
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;

        if (lockCursor)
            LockCursor();
        
    }

    public void LockCursor()
    {
        // 커서를 숨기고 고정
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        
        // 원하는 목표값을 기반으로 스크립트에 제한을 설정할 수 있게 함
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // 더 민감한 마우스에서 더 깔끔한 읽기 위한 원시 마우스 입력 가져옴
        mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // 민감도 설정에 대한 입력을 스케일링하고 그것을 스무딩 값에 곱함
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // 스무딩 델타를 적용하여 시간 경과에 따라 마우스 이동 보정
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        // 절대 마우스 이동 값(0점에서부터) 찾음
        _mouseAbsolute += _smoothMouse;

        // 로컬 x 값을 먼저 제한하고 적용하여 월드 변환에 영향을 받지 않게 함
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        // 그런 다음 글로벌 y 값을 제한하고 적용
        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        // 카메라의 부모 역할을 하는 캐릭터 본체가 있는 경우
        if (characterBody)
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
            characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }
        else
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }

        
    }
}
*/