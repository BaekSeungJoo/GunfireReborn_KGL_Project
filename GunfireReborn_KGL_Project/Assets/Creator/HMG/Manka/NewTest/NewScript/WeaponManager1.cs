using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
public class WeaponManager1 : MonoBehaviourPun
{
    //현재 활성화되있는 슬롯을 확인하기위한변수
    public bool[] ActiveSlot;
    //현재 슬롯에 있는 웨폰을 확인하기위한 변수
    public string[] slotWeapons;
    //장착하지않은 웨폰들의 프리팹 저장배열
    public GameObject[] weaponPrefabs;
    //플레이어의 IK함수를 가져오기위한 변수
    private IK1 playerIK;
    //보이지않지만 팔에있는 웨폰오브젝트들을 껏다켰다 하기위해 만든배열
    public GameObject[] Equip_weapons;
    //1인칭 오브젝트들;
    public GameObject[] Front_weapons;
    //프런트플레이어의 IK함수를 가져오기위한 변수
    private FrontIK1 frontIK;
    //프런트 플레어을 지정하기위한변수
    public GameObject frontPlayer;
    // Update is called once per frame
    private Animator frontAnimator;


    private void Start()
    {
        //제일처음에는 슬롯0과 1에 아무것도 없는상태가되야한다.
        slotWeapons[0] = null;
        slotWeapons[1] = null;
        //슬롯2에 기본권총이있다.
        slotWeapons[2] = "Pistol";
        //액티브슬롯3을 만든다.
        ActiveSlot = new bool[3];
        //슬롯2를 활성화시킨다.
        ActiveSlot[2] = true;
        //플레이어ik ik를받는다.
        playerIK = gameObject.GetComponent<IK1>();
        //
        frontPlayer = Camera.main.transform.GetChild(0).gameObject;

        frontIK = frontPlayer.GetComponent<FrontIK1>();
        //Todo: Equip_weapons[]배열에 존재할모든 무기들을 추가해줘야한다.
        //Equp_weapons[]배열의 무기들의 순서는 프리팹저장배열의 순서와같아야한다.
        //프리팹 저장배열이 반드시 인스펙터창에서 설정이되어있어야한다.
        //만약 새로운 무기를 추가하고싶다면 Equip_weapons[]와 weaponPrefabs[]배열에 필수적으로 추가해야한다.

        //스왑을할때는 Activeslot[]을 변경해주고, ik, Equipweapons[]를 변경해야한다.

        for (int i = 0; i < Front_weapons.Length; i++)
        {
            Transform child = frontPlayer.transform.GetChild(2).GetChild(i); // 자식 게임 오브젝트 가져오기
            Front_weapons[i] = child.gameObject; // 배열에 넣기
        }
        for (int i =0; i < frontIK.FrontWeaponChilds.Length; i++)
        {
            Transform child = frontPlayer.transform.GetChild(2).GetChild(i);
            frontIK.FrontWeaponChilds[i] = child.gameObject;
        }

        frontIK.ChangeIK("Pistol");
        frontAnimator = frontPlayer.GetComponent<Animator>();
        frontIK.IKAnimator = frontAnimator;
        
    }
    void Update()
    {

        if(!photonView.IsMine)
        {
            return;
        }
        if (Input.GetButtonDown("Swap1"))
        {
            //슬롯1이 비어있으면 return
            if (slotWeapons[0] == null)
            {
                return;
            }
            //Todo : 2번, 혹은 3번이라면  현재 총을 쏘고있거나 장전중이라면 애니메이션을 취소하고

            if (ActiveSlot[0] == true)
            {
                return;
            }
            ActiveSlot[0] = true;
            ActiveSlot[1] = false;
            ActiveSlot[2] = false;
            frontAnimator.SetTrigger("Swap");
            StartCoroutine(DelayedWeaponChange(0)); // DelayedWeaponChange 코루틴을 시작하여 0.6초 후에 코드 블록 실행
        }
        else if(Input.GetButtonDown("Swap2"))
        {
            //슬롯2가 비어있다면 return;
            if (slotWeapons[1] == null)
            {
                return;
            }
            //Todo : 현재 사용하고있는 장비가 몇번슬롯에있는지 확인하고 2번이라면 그대로냅두고
            //1번, 혹은 3번이라면  현재 총을 쏘고있거나 장전중이라면 애니메이션을 취소하고
            //2번에 원래있던 무기를 다시활성화시키고 ik를 연결한다.
            if (ActiveSlot[1] == true)
            {
                return;
            }
            ActiveSlot[0] = false;
            ActiveSlot[1] = true;
            ActiveSlot[2] = false;
            frontAnimator.SetTrigger("Swap");
            StartCoroutine(DelayedWeaponChange(1));
        }
        else if(Input.GetButtonDown("Swap3"))
        {
            //Todo : 현재 사용하고있는 장비가 몇번슬롯에있는지 확인하고 3번이라면 그대로냅두고
            //1번, 혹은 2번이라면  현재 총을 쏘고있거나 장전중이라면 애니메이션을 취소하고
            //3번에 원래있던 무기를 다시활성화시키고 ik를 연결한다.
            if (ActiveSlot[2] == true)
            {
                return;
            }
            ActiveSlot[0] = false;
            ActiveSlot[1] = false;
            ActiveSlot[2] = true;
            frontAnimator.SetTrigger("Swap");
            StartCoroutine(DelayedWeaponChange(2));
        }
    }

    public void EquipWeapon(string weaponName,int First)
    {
        if(photonView.IsMine)
        {
            if (First == 0)
            {   //만약 첫번째 슬롯이 비어있어서 장착된경우라면
                Debug.Log("EquipWeapon1");
                Debug.LogFormat("{0}", weaponName);    
                //슬롯2,3은 비활성화한다.
                ActiveSlot[2] = false;
                ActiveSlot[1] = false;
                //슬롯1은 활성화한다.
                ActiveSlot[0] = true;
                //첫번째 슬롯의 아이템을 먹은 아이템으로 바꾼다.
                weaponName = weaponName.Replace("(get)(Clone)", "");
                slotWeapons[0] = weaponName;
                frontAnimator.SetTrigger("Swap");
                /* //또한 먹은아이템의 이름을 확인해서 IK로 바꾼다.
                 playerIK.ChangeIK(weaponName);
                 //Debug.Log("format1");
                 //1인칭시점의 IK또한바꿔준다.
                 frontIK.ChangeIK(weaponName);
                 //Debug.Log("format2");

                 //아닌 것들을 모두 false로 바꾸고 맞는것무기는 true로 바꾼다.
                 TurnWeapon(weaponName);*/
                Debug.LogFormat("{0}", weaponName);

                // ============= Leagcy : Shin ===============================

                StartCoroutine(GetWeapon(weaponName));

                // ============= Leagcy : Shin ===============================

                //photonView.RPC("GetWeapon_SSC", RpcTarget.All, weaponName);

            }
            else if (First == 1)
            {   //만약 두번째 슬롯이 비어있어서 장착된경우라면
                Debug.Log("EquipWeapon2");
                //3번째 슬롯이 활성화된상태라면
                //슬롯1,3은 비활성화한다.
                ActiveSlot[2] = false;
                ActiveSlot[0] = false;
                //슬롯2은 활성화한다.
                ActiveSlot[1] = true;
                //두번째 슬롯의 아이템을 먹은 아이템으로 바꾼다.
                weaponName = weaponName.Replace("(get)(Clone)", "");
                slotWeapons[1] = weaponName;
                frontAnimator.SetTrigger("Swap");
                /*   //또한 먹은아이템의 이름을 확인해서 IK로 바꾼다.
                   playerIK.ChangeIK(weaponName);
                   //1인칭시점의 IK또한바꿔준다.
                   frontIK.ChangeIK(weaponName);
                   //아닌 것들을 모두 false
                   TurnWeapon(weaponName);*/

                // ============= Leagcy : Shin ===============================

                StartCoroutine(GetWeapon(weaponName));

                // ============= Leagcy : Shin ===============================
                //photonView.RPC("GetWeapon_SSC", RpcTarget.All, weaponName);


            }
            else
            {
                Debug.Log("EquipWeapon");
                //만약 1,2번슬롯이 모두 무기가 장착되어있는 경우라면
                //현재 활성화된 슬롯이 몇번슬롯인지 체크하고
                //그슬롯의 아이템을 먹은 아이템으로 바꾼다.
                weaponName = weaponName.Replace("(get)(Clone)", "");
                slotWeapons[CheckActiveslot()] = weaponName;
                frontAnimator.SetTrigger("Swap");


                // ============= Leagcy : Shin ===============================

                StartCoroutine(GetWeapon(weaponName));

                // ============= Leagcy : Shin ===============================

                //photonView.RPC("GetWeapon_SSC", RpcTarget.All, weaponName);


                /* //또한 먹은아이템의 이름을 확인해서 IK로 바꾼다.
                 playerIK.ChangeIK(weaponName);
                 //1인칭시점의 IK또한바꿔준다.
                 frontIK.ChangeIK(weaponName);
                 //그리고 팔에있는 무기를 활성화시킨다.
                 Equip_weapons[SearchWeapon()].SetActive(true);
                 //1인칭 시점 무기도 교체한다.
                 Front_weapons[SearchWeapon()].SetActive(true);*/
                //
            }
        }

    }

    public int CheckActiveslot()
    {   //현재 활성화된 슬롯을 체크하는함수
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
        for(int i=0; i<Equip_weapons.Length;i++)
        {
            if (slotWeapons[CheckActiveslot()] == Equip_weapons[i].name)
            {
                return i;
            }
        }
        return 999;
    }

    public void TurnWeapon(string weaponName)
    {// 손에있는 무기배열중 아닌것을 모두끄는 함수.

        if (photonView.IsMine)
        { 
            for(int i =0; i< Equip_weapons.Length; i++)
            {
                if (Equip_weapons[i].name == weaponName)
                {
                    photonView.RPC("LastChange_T", RpcTarget.All, i);
                    //Equip_weapons[i].SetActive(true);
                    Debug.LogFormat("{0},{1}", Equip_weapons[i].name, Front_weapons[i].name);
                    Front_weapons[i].SetActive(true);
                }
                else
                {
                    photonView.RPC("LastChange_F", RpcTarget.All, i);
                    //Equip_weapons[i].SetActive(false);
                    Front_weapons[i].SetActive(false);
                }
            }
        }
    }


    IEnumerator DelayedWeaponChange(int number)
    {
        yield return new WaitForSeconds(0.6f);

        playerIK.ChangeIK(slotWeapons[number]);
        frontIK.ChangeIK(slotWeapons[number]);
        TurnWeapon(slotWeapons[number]);
    }
    [PunRPC]
    public void LastChange_T(int i)
    {
        Equip_weapons[i].SetActive(true);
    }

    [PunRPC]
    public void LastChange_F(int i)
    {
        Equip_weapons[i].SetActive(false);
    }
    IEnumerator GetWeapon(string weaponName)
    {
        if (photonView.IsMine)
        {
            yield return new WaitForSeconds(0.6f); // 0.6초 대기

            //또한 먹은아이템의 이름을 확인해서 IK로 바꾼다.
            playerIK.ChangeIK(weaponName);
            //Debug.Log("format1");
            //1인칭시점의 IK또한바꿔준다.
            frontIK.ChangeIK(weaponName);
            //Debug.Log("format2");

            //아닌 것들을 모두 false로 바꾸고 맞는것무기는 true로 바꾼다.

            //photonView.RPC("TurnWeapon", RpcTarget.All, weaponName);

            // =============== Legacy : Shin =====================

            TurnWeapon(weaponName);

            // =============== Legacy : Shin =====================
        }

    }


    public void GetWeapon_SSC(string weaponName)
    {
        StartCoroutine(GetWeapon(weaponName));
    }
}
