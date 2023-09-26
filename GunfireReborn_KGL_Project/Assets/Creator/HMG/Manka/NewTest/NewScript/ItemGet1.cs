using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using System.Xml;

public class ItemGet1 : MonoBehaviourPun
{
    GameObject nearObject;

    Item nearItem;
    //플레이어 웨폰슬롯
    WeaponManager1 weaponManager;

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
        cam = FindObjectOfType<CinemachineVirtualCamera>();
        weaponManager = GetComponent<WeaponManager1>();
    }

    // Update is called once per frame
    void Update()
    {

        if(!photonView.IsMine)
        {
            return;
        }

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
        //Debug.Log("샷건에 닿은거같은데?");
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
        //ToDo : if문으로 만약 공격중이라면 바로 return하도록 만들어야한다.
        //공격중에는 아이템을 먹을수가없다.
        if (nearObject != null)
        {
            //Debug.LogFormat("{0}", nearObject.name);
            if (nearObject.tag == "weapon" && Input.GetButtonDown("Get"))
            {//가까이 있고, 아이템 갖는 입력을받고, 레이캐스트에 맞았을때
             //일단은 임시적으로 아이템을 파괴하게 만들음

                ChangeWeapon();
                Debug.LogFormat("왜 안되는거야?");
                Destroy(nearObject.gameObject);
                Debug.LogFormat("파괴가 안된다고?");
            }
        }
    }

    private void ChangeWeapon()
    {
        if(photonView.IsMine)
        {
            if (weaponManager.slotWeapons[0] == null)
            {//만약 1번슬롯이 비어있다면  1번에 무기장착하도록함.
                Debug.Log("1번슬롯에입장");
                weaponManager.EquipWeapon(nearObject.name, 0);
            }
            else if (weaponManager.slotWeapons[1] == null)
            {//만약 2번슬롯이 비어있다면  2번에 무기장착하도록함.
                Debug.Log("2번슬롯에입장");
                weaponManager.EquipWeapon(nearObject.name, 1);
            }
            else
            {//만약 1,2,번 슬롯이 모두 있다면 지금현재 사용하고있는 슬롯의 무기와 교환하도록함.
                Debug.Log("1,2번중 활성화된 슬롯에 입장");
                weaponManager.EquipWeapon(nearObject.name, 99); //어차피 0과 1이외의 경우는 EquipWeapon에서 현재 활성화되어있는 슬롯을체크하고 그슬롯을 바꾸기때문에 상관없다고판단했다.
            }
        }

    }
}
