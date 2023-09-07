using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class RayCastShot : MonoBehaviour
{
    //레이를 그릴 렌더러
    private LineRenderer RayLineRenderer;

    //레이를 쏘기시작할 캠
    public CinemachineVirtualCamera cam;

    //레이 변수
    public RaycastHit hitInfo;

    //레이의 사정거리
    private float rayDistance = 100f;

    //레이가 맞은곳을 저장할변수
    Vector3 hitPosition = Vector3.zero;

    void Start()
    {
        RayLineRenderer = GetComponent<LineRenderer>();

        //사용할 점을 두개로 변경
        RayLineRenderer.positionCount = 2;
        //라인렌더러를 활성화
        //RayLineRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        ShotRaycast();
    }

    private void ShotRaycast()
    {
        

        //레이캐스트 발사
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo,rayDistance))
        {   //레이가 충돌한경우


            //레이가 충돌한 위치저장
            hitPosition = hitInfo.point;

        }
        else
        {   //레이가 아무것도 맞지않은경우
            hitPosition = cam.transform.position + cam.transform.forward * rayDistance;
        }

        //레이캐스트 라인렌더러 그리는 함수

        RayLineDraw();

    }

    private void RayLineDraw()
    {
        //선의 시작점은 캠의 위치
        RayLineRenderer.SetPosition(0, cam.transform.position);
        //선의 끝점은 레이의 충돌위치
        RayLineRenderer.SetPosition(1, hitPosition);

    }
}
