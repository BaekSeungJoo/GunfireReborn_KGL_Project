using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGet : MonoBehaviour
{

    GameObject nearObject;

    Item nearItem;

    PlayerInput input;


    //레이를 쏘기시작할 캠
    public CinemachineVirtualCamera cam;

    //레이 변수
    public RaycastHit hitInfo;

    //레이의 사정거리
    private float rayDistance = 10f;

    //레이가 맞은곳을 저장할변수
    Vector3 hitPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        ShotRaycast();
    }
    private void ShotRaycast()
    {
        

        //레이캐스트 발사
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, rayDistance))
        {   //레이가 충돌한경우
            Debug.DrawRay(cam.transform.position, cam.transform.forward * rayDistance, Color.red);
            if (hitInfo.transform.CompareTag("weapon"))
            {
                GetItem();
            }

        }
        else
        {   //레이가 아무것도 맞지않은경우
            hitPosition = cam.transform.position + cam.transform.forward * rayDistance;
        }
        


    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "weapon")
        {
            //무기 트리거에 닿았을때 nearObject에 무기를담는다.
            nearObject = other.gameObject;
            nearItem = nearObject.GetComponent<Item>(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "weapon")
        {
            //무기 트리거를 벗어났을때 nearObject를  null로 비운다.
            nearObject = null;
        }
    }

    private void GetItem()
    {
        
        if (nearObject != null)
        {
            Debug.LogFormat("{0}", nearObject.name);
            if (nearObject.tag == "weapon" && input.get == true)
            {//가까이 있고, 아이템 갖는 입력을받고, 레이캐스트에 맞았을때
             //일단은 임시적으로 아이템을 파괴하게 만들음
                Destroy(nearObject.gameObject);
                input.get = false;
            }
        }
       
    }
}
