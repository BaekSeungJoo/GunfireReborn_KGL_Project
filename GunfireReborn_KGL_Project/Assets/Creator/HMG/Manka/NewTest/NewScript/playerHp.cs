
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class playerHp : MonoBehaviourPun
{
    public int maxHealth;       //최대HP
    public int curHealth;       //현재HP
    public int maxShield;       //최대shield;
    public int curShield;       //현재shield;

    public bool isturnGroggy;    //그로기상태가됐는지 확인하는변수
    public bool isturnPlay;      //플레이상태가됐는지 확인하는 변수

    public Slider hpBar;                        //플레이hp바
    public Slider shieldBar;                    //플레이쉴드바
    private Animator animator;                  //플레이어 애니메이터
    private IK1 ik;                             //3인칭 ik를 받아오는 변수
    private CinemachineVirtualCamera virtualCam;//내 시네머신을 받아오는변수, //레이를 쏘기시작할 캠
    private PlayerRoation roation;              //플레이어의 회전을 담당하는스크립트;
    public Transform weapon;                    //3인칭플레이어의 무기들
    private FrontIK1 frontIK;                   //1인칭 ik를 받아오는 변수
    public Transform frontWeapon;               //1인칭 플레어의 무기들

    public GameObject recoveryBarOB;            //회복바 false true로 바꾸게 하기위해서
    public Image recoveryBar;                   //회복바 Slider 내가쓰러진경우 보이게할 바
    private float dieSpeed = 1f;                 //죽어가는속도
    private float recoverySpeed = 1;             //회복속도
    private float currentRecovery = 1.0f;       //현재 회복진행도
    public RaycastHit hitInfo;                  //레이정보를 담을변수
    private float rayDistance = 5f;             //레이의 사정거리
    public bool isCure;                         //회복중임을 체크하는 변수
    public List<GameObject> otherPlayers;       //내 주변에 있는 플레이어들을 담을 변수
    private GameObject hPlayer;                 //내가 회복시키고있는 플레이어를 담을 변수

    public enum State
    {
        play,
        groggy,
        die
    }

    public State state;
    private void Start()
    {
        //초기화
        roation = GetComponent<PlayerRoation>();                                //회전스크리브 받아오기
        hpBar = GameObject.Find("HpBgBar").GetComponent<Slider>();              //hp바 받아오기
        shieldBar = GameObject.Find("ShieldBgBar").GetComponent<Slider>();      //shield바 받아오기
        virtualCam = FindObjectOfType<CinemachineVirtualCamera>();              //버츄얼캠 받아오기
        ik = GetComponent<IK1>();                                               //3인칭ik 받아오기
        animator = GetComponent<Animator>();                                    //내 애니메이터 받아오기
        frontIK = GetComponent<FrontIK1>();                                     //1인칭ik 받아오기
       
        state = State.play;                                                     //현재 상태를 play로 해놓음
        curHealth = maxHealth;                                                  //현재HP를 MAXHP로 초기화
        curShield = maxShield;                                                  //현재쉴드를 MAXShield로 초기화
        hpBar.value = (float)curHealth / (float)maxHealth;                      //hp바의 초기화
        shieldBar.value = (float)curShield / (float)maxShield;                  //Shield바의 초기화

        otherPlayers = new List<GameObject>();                                  //주변의플레이어들을 담을 리스트
        recoveryBar.fillAmount = 1;                                             //리커버리바 
        currentRecovery = 0.99f;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (state == State.groggy)
        {
            Debug.Log("실행되고이써?");
            photonView.RPC("GoingDead", RpcTarget.All);
        }
        //RPC로 전해줘야하는부분
        photonView.RPC("StateUpdate", RpcTarget.All, curHealth, curShield);     //상태를 업데이트하는 함수,hp,shield,애니메이터파라미터,
        //RPC로 전해주지않아도된는, 전해주면안되는 부분
        if (state == State.groggy)                              // 그로기상태라면 
        {
            animator.SetBool("groggy", true);                   // 애니메이터 파라미터를 groggy로 만들어서 groggy애니메이션이 재생되게만들음
        }
        else                                                    // 플레이상태라면
        {
            animator.SetBool("groggy", false);                  // 애니메이터 파라미터를 groggy로 만들어서 groggy애니메이션이 재생되지않게만들음
            ShotRayCast();                                      // 플레이중이라면 rayCast를 쏘게만들음
        }
        if (state == State.die)                                 // 죽은상태라면 
        {
            animator.SetTrigger("Dead");                        // 죽음 애니메이션을 재생되게함
        }
    }
    
    #region 피격함수
    //player가 공격받았을때 원격 프로시저 콜 함수
    public void PlayerTakeDamage(int damage)
    {
        Debug.LogFormat("죽어가요~");
        //쉴드가 남아있다면 쉴드가 까이게 하고 쉴드가 0이거나 이하라면 hp가 까이게함
        if (curShield <= 0)
        {
            curHealth -= damage;
        }
        else
        {
            curShield -= damage;
        }
        //현재 hp가 0보다 낮게된다면 플레이어를 그로기상태로만들음
        if (curHealth <= 0)
        {
            state = State.groggy;
            TurnGroggy();
        }
        //포톤네트워크마스터 클라이언트로   쉴드와 hp를 다른월드의 내게 적용시킴
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("PlayerHealthUpdated", RpcTarget.Others, curHealth, curShield);
        }
    }
    #endregion
    #region 상태업데이트함수
    [PunRPC]
    private void StateUpdate(int newHealth, int newShield)
    {
        //hp바와 shield바를 지속적으로 업뎃해줌
        shieldBar.value = (float)curShield / (float)maxShield;
        hpBar.value = (float)curHealth / (float)maxHealth;

        curHealth = newHealth;
        curShield = newShield;

        if (state == State.groggy)                              // 그로기상태라면 
        {
            Debug.LogFormat("access complete");
            roation.enabled = false;                            // 플레이어의 로테이션을 꺼버림
            ik.enabled = false;                                 // ik애니메이터를 꺼버림
            recoveryBarOB.SetActive(true);                            // 큐어 바를 활성화시킴
        }
        else                                                    // 그로기상태가 아니라면
        {
            roation.enabled = true;                             // 플레이어의 로테이션을 켜버림
            ik.enabled = true;                                  // ik애니메이터를 꺼버림
            recoveryBarOB.SetActive(false);                           // 큐어 바를 활성화시킴
        }
    }
    #endregion
    #region 무기를 비활성화하는함수
    private void FalseWeapons()
    {
        for (int i = 0; i < weapon.childCount; i++)
        {
            weapon.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void FalseFrontWeapons()
    {
        for (int i =0; i < frontWeapon.childCount; i++)
        {
            frontWeapon.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion
    #region 그로기상태로 변하는 함수와, 플레이상태로 변하는 함수
    private void TurnGroggy()
    {
        isturnGroggy = true;
        virtualCam.Follow = gameObject.transform;           // 버츄얼카메라의 Follow와 LookAt을 나로 조절해줌
        virtualCam.LookAt = gameObject.transform;           // 이걸 밖으로 빼야함 왜냐면 StateUpdate는 Rpc로 주기적으로 줘야하는데 다른월드의 나는 버츄얼카메라가 없기때문이다.

        virtualCam.transform.parent = null;

        virtualCam.AddCinemachineComponent<CinemachineOrbitalTransposer>();
        CinemachineOrbitalTransposer v = virtualCam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        v.m_FollowOffset.y = 1f;                            // 카메라 변경
        v.m_FollowOffset.x = 1f;
        v.m_FollowOffset.z = 1.5f;
        v.m_XAxis.m_MaxSpeed = 1000;

        roation.enabled = false;                            // 회전스크립트를 false로 만든다.

        FalseWeapons();                                     // 3인칭 무기 모두 false로 만들기
        ik.enabled = false;                                 // 3인칭 ik false로 만들기
        frontIK.enabled = false;                            // 1인칭 ik false로 만들기
        FalseFrontWeapons();                                // 1인칭 무기 모두 false로 만들기
    }

    public void TurnPlay()
    {
        virtualCam.Follow = null;                           // 버츄얼카메라의 Follow와 LookAt을 나로 조절해줌
        virtualCam.LookAt = null;                           // 이걸 밖으로 빼야함 왜냐면 StateUpdate는 Rpc로 주기적으로 줘야하는데 다른월드의 나는 버츄얼카메라가 없기때문이다.
        virtualCam.transform.parent = transform.transform;  // 카메라를 다시 캐릭터의 하위 오브젝트로 넣는다.
        virtualCam.DestroyCinemachineComponent<CinemachineOrbitalTransposer>();
        roation.enabled = true;
        virtualCam.transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.8f, transform.localPosition.z + 0.4f);
        isturnPlay = true;
    }
    #endregion
    #region 죽어가는함수
    [PunRPC]
    private void GoingDead()
    {
        if (isCure == false)
        {
            
            currentRecovery -= dieSpeed * Time.deltaTime;
            if (currentRecovery < 0)
            {
                currentRecovery = 0;
            }
            recoveryBar.fillAmount = currentRecovery/1;
            Debug.LogFormat("{0}", currentRecovery);
        }
    }
    #endregion
    #region 트리거 함수
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            otherPlayers.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            otherPlayers.Remove(other.gameObject);
        }
    }
    #endregion
    #region 가까이있는 플레이어가 있는지 판별하는 함수
    private int SearchPlayer()
    {
        int peopleNum = 0;
        for (int i = 0; i < otherPlayers.Count; i++)
        {
            if (otherPlayers[i] != null)
            {
                peopleNum++;
            }
        }
        if (peopleNum == 0)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
    #endregion
    #region 치료하는 함수
    [PunRPC]
    private void CureOther()
    {
        //현재 치유중 여부를 나타내는 변수를 true로 만들음 
        playerHp otherHP = hPlayer.GetComponent<playerHp>();
        isCure = true;
        otherHP.currentRecovery += otherHP.recoverySpeed * Time.deltaTime;
        otherHP.recoveryBar.fillAmount = otherHP.currentRecovery;
        if (otherHP.currentRecovery >= 1.0f)
        {
            // 회복이 완료되면 부활 로직을 호출하거나 다른 필요한 작업을 수행합니다.
            otherHP.curHealth = otherHP.maxHealth / 2;
            isCure = false;
            hPlayer = null;
            //플레이어의 스테이트가 play가되면 playerCure이 꺼지므로 반드시 제일 마지막에 두어야한다.
            otherHP.state = playerHp.State.play;
            otherHP.TurnPlay();
        }
    }
    #endregion
    #region 중간에 치료를 멈출때 함수
    [PunRPC]

    private void StopCure()
    {
        isCure = false;
    }
    #endregion
    #region 레이를 쏘는함수
    private void ShotRayCast()
    {
        if (SearchPlayer() == 2)
        {//주변에 플레이어가 있을경우
         //여기서 레이캐스트를 발사해서 그로기 상태인 플레이어일경우 cureOther()를 사용하게만들음\
            if (Physics.Raycast(virtualCam.transform.position, virtualCam.transform.forward, out hitInfo, rayDistance))
            {   //레이가 충돌한경우 hitInfo에 충돌 정보를 받아온다.
                Debug.DrawRay(virtualCam.transform.position, virtualCam.transform.forward * rayDistance, Color.blue);
                if (hitInfo.transform.CompareTag("Player") && hitInfo.collider.gameObject.GetComponent<playerHp>().state == playerHp.State.groggy)
                {   //맞은물체의 Tag가 플레이어고 맞은 플레이어가 그로기 상태라면
                    if (Input.GetKey("Get"))
                    {   //f를누르고있으면
                        hPlayer = hitInfo.collider.gameObject;
                        //지금 레이를 맞은 플레이어를 회복하고있는 플레이어 변수에 저장한다.
                        photonView.RPC("CureOther", RpcTarget.All);
                        //cureOther함수를 RPC한다.
                    }
                    else
                    {
                        photonView.RPC("StopCure", RpcTarget.All);
                        //회복 중단 시 회복 상태를 초기화합니다.
                    }
                }
            }
            else
            {//만약 레이가 플레이어를 벗어난다면 지금치료중임을 나타내는 변수를 false로 만든다.
                isCure = false;
            }
        }
    }
    #endregion
}
