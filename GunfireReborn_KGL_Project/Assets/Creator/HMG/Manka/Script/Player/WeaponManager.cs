using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //현재 활성화되있는 슬롯을 확인하기위한변수
    public bool[] ActiveSlot;
    //현재 슬롯에 있는 웨폰을 확인하기위한 변수
    public string[] slotWeapons;
    //모든 종류의 웨폰들
    public string[] weapons;
    //장착하지않은 웨폰들의 프리팹 저장배열
    public GameObject[] weaponPrefabs;
    //플레이어의 IK함수를 가져오기위한 변수
    private IK playerIK;
    //보이지않지만 팔에있는 웨폰오브젝트들을 껏다켰다 하기위해 만든배열
    public GameObject[] Equip_weapons;

    // Update is called once per frame


    private void Start()
    {
        playerIK = gameObject.GetComponent<IK>();
        weapons[0] = "Pistol";
        weapons[1] = "Rifle";
        //Todo: weapons[]배열에 넣을것들 추가해줘야한다.
        //weapons배열의 무기들의 순서는 프리팹저장배열의 순서와같아야한다. 또한 Equip_weapons의 순서와도같아야한다.
        //프리팹 저장배열이 반드시 인스펙터창에서 설정이되어있어야한다.

        //만약 새로운 무기를 추가하고싶다면 weapons와 weaponPrefabs배열에 필수적으로 추가해야한다.
    }
    void Update()
    {
        if(Input.GetButtonDown("Swap1"))
        {
            //Todo : 현재 사용하고있는 장비가 몇번슬롯에있는지 확인하고 1번이라면 그대로냅두고
            //2번, 혹은 3번이라면  현재 총을 쏘고있거나 장전중이라면 애니메이션을 취소하고
            //1번에 원래있던 무기를 다시활성화시키고 ik를 연결한다.
        }
        else if(Input.GetButtonDown("Swap2"))
        {
            //
        }
        else if(Input.GetButtonDown("Swap3"))
        {

        }
    }

    public void EquipWeapon(string weaponName,int First)
    {
        if (First == 0)
        {   //만약 첫번째 슬롯이 비어있어서 장착된경우라면
            //첫번째 슬롯의 아이템을 먹은 아이템으로 바꾼다.
            slotWeapons[0] = weaponName;
            //또한 먹은아이템의 이름을 확인해서 IK로 바꾼다.
            playerIK.ChangeIK(weaponName);
            //그리고 팔에있는 무기를 활성화시킨다.
            Equip_weapons[SearchWeapon()].SetActive(true);
            //
        }
        else if (First == 1)
        {   //만약 두번째 슬롯이 비어있어서 장착된경우라면
            //두번째 슬롯의 아이템을 먹은 아이템으로 바꾼다.
            slotWeapons[1] = weaponName;
            //또한 먹은아이템의 이름을 확인해서 IK로 바꾼다.
            playerIK.ChangeIK(weaponName);
            //그리고 팔에있는 무기를 활성화시킨다.
            Equip_weapons[SearchWeapon()].SetActive(true);
            //
        }
        else
        {
            //만약 1,2번슬롯이 모두 무기가 장착되어있는 경우라면
            //현재 활성화된 슬롯이 몇번슬롯인지 체크하고
            //그슬롯의 아이템을 먹은 아이템으로 바꾼다.
            slotWeapons[CheckActiveslot()] = weaponName;
            //또한 먹은아이템의 이름을 확인해서 IK로 바꾼다.
            playerIK.ChangeIK(weaponName);
            //그리고 팔에있는 무기를 활성화시킨다.
            Equip_weapons[SearchWeapon()].SetActive(true);
            //
        }
    }

    public int CheckActiveslot()
    {
        //아무무기도 안먹은상태는 기본무기3번슬롯이 활성화되야하기에 2가 기본값이다.
        int ActiveSlotNum=2;
        for(int i =0; i<3; i++)
        {
            if(ActiveSlot[i] == true)
            {
                ActiveSlotNum = i;
            }
        }

        return ActiveSlotNum;
    }

    public void WeaponDrop()
    {
        int ActiveWeaponNum = 999;
        //장착하고있는 슬롯의 웨폰을 확인한다.
        ActiveWeaponNum = SearchWeapon();

        if (ActiveWeaponNum != 999)
        {//만약 내가가지고있는 무기가 전체무기리스트에 있다면  (만약 없다면 999가됨)
         //prefabs배열에서 그 인덱스번호를 instantiate한다.

            // 플레이어의 현재 위치를 가져온다. (예: transform.position을 사용)
            Vector3 playerPosition = transform.position;

            // 플레이어의 현재 위치에서 z 좌표를 +2로 이동한 위치에 Instantiate한다.
            Vector3 spawnPosition = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z + 2);

            // Instantiate 함수를 사용하여 무기를 생성한다.
            Instantiate(weaponPrefabs[ActiveWeaponNum], spawnPosition, Quaternion.identity);
            // 마지막으로 내손에 있던 무기를 false로 비활성화시킨다.
            Equip_weapons[ActiveWeaponNum].SetActive(false);
        }
        else
        {
            Debug.LogFormat("내가가지고있는 무기가 전체 무기배열에 없다.");
        }
    }

    public int SearchWeapon()
    {// 내가 가지고있는 무기가 전체리스트의 몇번인덱스에 있는지 확인하는함수. 없다면 999를 반환한다.
        for(int i=0; i<weapons.Length;i++)
        {
            if (slotWeapons[CheckActiveslot()] == weapons[i])
            {
                return i;
            }
        }
        return 999;
    }
}
