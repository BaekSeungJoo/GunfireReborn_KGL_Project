
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class playerHp : MonoBehaviourPun //,IPunObservable
{
    private int maxHealth;       //최대HP
    public int curHealth;       //현재HP
    private int maxShield;       //최대shield;
    private float curShield;       //현재shield;

    public bool isturnGroggy;    //그로기상태가됐는지 확인하는변수
    public bool isturnPlay;      //플레이상태가됐는지 확인하는 변수

    private Image hpBar;                       //플레이hp바
    private Image shieldBar;                   //플레이쉴드바
    private Animator animator;                  //플레이어 애니메이터
    private IK1 ik;                             //3인칭 ik를 받아오는 변수
    private CinemachineVirtualCamera virtualCam;//내 시네머신을 받아오는변수, //레이를 쏘기시작할 캠
    private PlayerRoation roation;              //플레이어의 회전을 담당하는스크립트;
    public Transform weapon;                    //3인칭플레이어의 무기들
    private GameObject FPSUnityChan;            //1인칭유니티짱
    public GameObject recoveryBarOB;            //회복바 false true로 바꾸게 하기위해서
    public Image recoveryBar;                   //회복바 Slider 내가쓰러진경우 보이게할 바
    private float dyingTime = 20.0f;            //죽는데 걸리는시간
    private float dieSpeed = 1f;                //죽어가는속도
    private float cureTime = 5f;                //하는데 걸리는시간
    private float recoverySpeed = 1;            //회복하는속도
    public RaycastHit hitInfo;                  //레이정보를 담을변수
    private float rayDistance = 5f;             //레이의 사정거리
    public bool isCure;                         //회복중임을 체크하는 변수
    private GameObject hPlayer;                 //내가 회복시키고있는 플레이어를 담을 변수
    private bool isDead;                        //죽은지 체크하는 변수

    private float shieldRecharge = 10f;     //1초마다 회복될 쉴드량
    private float shieldRechargeCool = 5f;      //쉴드회복 시작 쿨타임
    private float rechargeTimer = 0;            //쉴드회복 타이머 초기화

   /* private Camera Vcamera;
    public GameObject cross;
    public GameObject cube;*/

    [SerializeField]
    private bool activeRecoveryBar;
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
        hpBar = GameObject.Find("HPBar").GetComponent<Image>();                 //hp바 받아오기
        shieldBar = GameObject.Find("ShieldBar").GetComponent<Image>();         //shield바 받아오기
        virtualCam = FindObjectOfType<CinemachineVirtualCamera>();              //버츄얼캠 받아오기
        ik = GetComponent<IK1>();                                               //3인칭ik 받아오기
        animator = GetComponent<Animator>();                                    //내 애니메이터 받아오기
        FPSUnityChan = Camera.main.transform.GetChild(0).gameObject;            //1인칭ik 받아오기

        maxHealth = 100;                                                        //최대 hp설정
        maxShield = 100;                                                        //최대 shield설정
        state = State.play;                                                     //현재 상태를 play로 해놓음
        curHealth = maxHealth;                                                  //현재HP를 MAXHP로 초기화
        curShield = maxShield;                                                  //현재쉴드를 MAXShield로 초기화
        hpBar.fillAmount = (float)curHealth / (float)maxHealth;                 //hp바의 초기화
        shieldBar.fillAmount = (float)curShield / (float)maxShield;             //Shield바의 초기화

        recoveryBar.fillAmount = 1;                                             //리커버리바 1로 초기화
        isturnGroggy = true;                                                    //isturnGroggy true로 초기화  //state가 변할때 실행되는 함수들이 한번만 실행되게 만드는 변수들
        isturnPlay = true;                                                      //isturnPlayr true로 초기화   //만약 true로 안해놓으면 함수들이 한번실행되서 hp가 반밖에없는 상태로 시작함.
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        //hp와 shield 업데이트
        shieldBar.fillAmount = (float)curShield / (float)maxShield;
        hpBar.fillAmount = (float)curHealth / (float)maxHealth;

        #region 쉴드 회복 
        // 쉴드 회복 타이머를 증가시킴
        rechargeTimer += Time.deltaTime;

        // 5초 경과 시 쉴드 회복
        if (rechargeTimer >= shieldRechargeCool)
        {
            //Todo : 1초마다 쉴드 10씩 회복하게만들기
            curShield += Time.deltaTime * shieldRecharge;
        }
        // 쉴드 최대값 제한
        curShield = Mathf.Clamp(curShield, 0, maxShield); // maxShieldValue는 쉴드 최대값
        #endregion
/*
        if(activeRecoveryBar == true)
        {
            recoveryBarOB.SetActive(true);
        }
        else
        {
            recoveryBarOB.SetActive(false);
        }*/


        //그로기 상태일떄
        if (state == State.groggy)
        {
            Debug.Log("그로기냐");
            Debug.Log(isturnGroggy);
            if (isturnGroggy == false)
            {
               
                Debug.Log(isturnGroggy);
                Debug.Log("들어오니");
                animator.SetBool("groggy", true);                                    // 애니메이터 파라미터를 groggy로 만들어서 groggy애니메이션이 재생되게만들음
                roation.enabled = false;                                             // 3인칭 플레이어의 로테이션을 꺼버림
                photonView.RPC("MakeTrueRecoveryBar", RpcTarget.All);                // 큐어 바를 활성화시키고 ik를 끈다. rpc로 다른 월드도 키게만들음
                
                isturnGroggy = true;                                                 // isturnGroggy를 true로 만들어버려서 함수가 여러번실행되지않게함
            }

            photonView.RPC("GoingDead",RpcTarget.MasterClient);                                                             // recoverybar의 fillamount가 점점 줄어들게 만들음
        }
        // 플레이상태라면
        if (state == State.play)                                
        {
            
            if (isturnPlay == false)
            {
                animator.SetBool("groggy", false);                                   // 애니메이터 파라미터를 groggy로 만들어서 groggy애니메이션이 재생되지않게만들음
                TurnPlay();
                roation.enabled = true;                                              // 플레이어의 로테이션을 켜버림
                photonView.RPC("MakeFalseRecoveryBar", RpcTarget.All);               // 큐어 바를 비활성화시키고 ik를 다시 활성화시킨다.
                virtualCam.transform.parent = gameObject.transform;
                virtualCam.transform.localPosition = Vector3.zero;
                Debug.Log(virtualCam.transform.localPosition);
                virtualCam.transform.localPosition = new Vector3(0f,0.8f,0.4f);
                //virtualCam.transform.localPosition = new Vector3(0f, transform.localPosition.y + 0.8f, 0.4f);
                Debug.Log(virtualCam.transform.localPosition);
            }
            ShotRayCast();                                                           // 플레이중이라면 rayCast를 쏘게만들음
        }

        // 죽은상태라면 
        if (state == State.die)                                 
        {
            if (isDead == false)
            {
                animator.SetTrigger("Dead");                                         // 죽음 애니메이션을 재생되게함
                isDead = true;
            }
            ik.enabled = false;                   
        }
    }

    [PunRPC]
    private void MakeTrueRecoveryBar()
    {
        Debug.Log("MakeTrue");
        //activeRecoveryBar = true;
        recoveryBarOB.SetActive(true);
    }

    [PunRPC]
    private void MakeFalseRecoveryBar()
    {
       // activeRecoveryBar = false;
        recoveryBarOB.SetActive(false);
    }

    #region 피격함수
    //player가 공격받았을때 원격 프로시저 콜 함수
    //[PunRPC]
    public void PlayerTakeDamage(int damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (curShield <= 0)
            {
                curHealth -= damage;
            }
            else
            {
                curShield -= damage;
            }
            rechargeTimer = 0;
            photonView.RPC("PlayerHealthUpdated", RpcTarget.All, curHealth, curShield, rechargeTimer);
        }
        //쉴드가 남아있다면 쉴드가 까이게 하고 쉴드가 0이거나 이하라면 hp가 까이게함
        //현재 hp가 0보다 낮게된다면 플레이어를 그로기상태로만들음
       
        //포톤네트워크마스터 클라이언트로 쉴드와 hp를 다른월드의 내게 적용시킴
    }
    #endregion
   

    [PunRPC]
    private void PlayerHealthUpdated(int newCurHealth, float newCurShield,float rechargeTime)
    {
        curHealth = newCurHealth;
        curShield = newCurShield;
        rechargeTimer = rechargeTime;
        if (curHealth <= 0)
        {
            photonView.RPC("TurnTagStateGroggy", RpcTarget.All);        //태그
            isturnGroggy = false;
            isturnPlay = false;
            TurnGroggy();
        }
    }
    [PunRPC]
    private void TurnTagStateGroggy()
    {
        state = State.groggy;
        gameObject.tag = "Groggy";
    }

   
    
    #region 그로기상태로 변하는 함수와, 플레이상태로 변하는 함수
    private void TurnGroggy()
    {
        if(!photonView.IsMine)
        {
            return;
        }

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

        photonView.RPC("FalseWeapons", RpcTarget.All);      // 3인칭 무기 모두 false로 만들기
        ik.isIk = false;                                // 3인칭 ik false로 만들기
        FPSUnityChan.SetActive(false);                      // 1인칭 유니티짱 끄기
    }

   
    public void TurnPlay()
    {
        curHealth = maxHealth / 2;
        virtualCam.Follow = null;                           // 버츄얼카메라의 Follow와 LookAt을 나로 조절해줌
        virtualCam.LookAt = null;                           // 이걸 밖으로 빼야함 왜냐면 StateUpdate는 Rpc로 주기적으로 줘야하는데 다른월드의 나는 버츄얼카메라가 없기때문이다.
        virtualCam.transform.parent = transform.transform;  // 카메라를 다시 캐릭터의 하위 오브젝트로 넣는다.
        virtualCam.DestroyCinemachineComponent<CinemachineOrbitalTransposer>();
        roation.enabled = true;
        virtualCam.transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.8f, transform.localPosition.z + 0.4f);
        ik.isIk = true;
        ik.ChangeIK("Pistol");
        isturnPlay = true;
    }
    #endregion

    #region 무기를 비활성화하는함수
    [PunRPC]
    private void FalseWeapons()
    {
        for (int i = 0; i < weapon.childCount; i++)
        {
            weapon.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion


    #region 죽어가는함수
    [PunRPC]
    private void GoingDead()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isCure == false)
            {
                // 10초 동안 감소해야 할 양을 계산합니다.
                float decreaseAmount = 1.0f / dyingTime * dieSpeed * Time.deltaTime;
                // fillAmount를 업데이트합니다.
                recoveryBar.fillAmount = Mathf.Max(recoveryBar.fillAmount - decreaseAmount, 0);
                photonView.RPC("UpdateRecoveryBarDead", RpcTarget.All,recoveryBar.fillAmount);
                // fillAmount가 0이면 필요한 처리를 수행합니다.
                if (recoveryBar.fillAmount == 0)
                {
                    //죽음으로 처리함.
                    state = State.die;
                }
            }
        }
    }
    #endregion

    [PunRPC]
    private void UpdateRecoveryBarDead(float fillAmount)
    {
        recoveryBar.fillAmount = fillAmount;
    }

    
    #region 레이를 쏘는함수
    private void ShotRayCast()
    {
        if (Physics.Raycast(virtualCam.transform.position, virtualCam.transform.forward, out hitInfo, rayDistance))
        {   //레이가 충돌한경우 hitInfo에 충돌 정보를 받아온다.
            Debug.DrawRay(virtualCam.transform.position, virtualCam.transform.forward * rayDistance, Color.black);
            //playerHp test = hPlayer.GetComponent<playerHp>();
            if (hitInfo.transform.CompareTag("Groggy"))
            {
                //맞은물체의 Tag가 그로기고(태그가 플레이어일시 몬스터가 계속 때려서 카메라가 흔들림) 맞은 플레이어가 그로기 상태라면
                if (Input.GetButton("Get"))
                {   //f를누르고있으면
                    hPlayer = hitInfo.collider.gameObject;
                    //지금 레이를 맞은 플레이어를 회복하고있는 플레이어 변수에 저장한다.
                    hPlayer.GetComponent<playerHp>().photonView.RPC("Cure", RpcTarget.All);
                    //플레이어의 cure함수를 실행한다.
                }
                else
                {
                    photonView.RPC("StopCure", RpcTarget.All);
                    //회복 중단 시 회복 상태를 초기화합니다.
                }
            }
            else
            {
                photonView.RPC("StopCure", RpcTarget.All);
            }
        }

    }
    #endregion
    #region 중간에 치료를 멈출때 함수
    [PunRPC]

    private void StopCure()
    {
       hPlayer.GetComponent<playerHp>().isCure = false;
    }
    #endregion

   
    [PunRPC]
    private void Cure()
    {
        // 현재 치유중 여부를 나타내는 변수를 true로 만들음 
        if (PhotonNetwork.IsMasterClient)
        {

            isCure = true;
            // 5초 동안 증가해야 할 양을 계산합니다.
            float increaseAmount = 1.0f / cureTime * recoverySpeed * Time.deltaTime;
            // fillAmount를 업데이트합니다.
            recoveryBar.fillAmount = Mathf.Max(recoveryBar.fillAmount + increaseAmount, 0);
            photonView.RPC("UpdateRecoveryBar", RpcTarget.All,recoveryBar.fillAmount);
            if (recoveryBar.fillAmount >= 1.0f)
            {
                photonView.RPC("TurnPlayMode", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    private void UpdateRecoveryBar(float fillAmount)
    {
        recoveryBar.fillAmount = fillAmount;
    }

    [PunRPC]

    private void TurnPlayMode()
    {
        isCure = false;                                     // 회복중임을 나타내는변수를 false로 만든다.
        gameObject.tag = "Player";                          // 태그를 플레이어로 만든다
        state = State.play;                                 // 플레이어의 스테이트를 play로 만든다.
        weapon.GetChild(0).gameObject.SetActive(true);      // 3인칭웨폰을 활성화 한다.
        FPSUnityChan.SetActive(true);                       // 1인칭 유니티짱 키기

    }

    #region 플레이어 부활
    private void ReStart()
    {
        gameObject.tag = "Player";
        state = State.play;
        weapon.GetChild(0).gameObject.SetActive(true);      // 3인칭웨폰을 활성화 한다.
        FPSUnityChan.SetActive(true);                       // 1인칭 유니티짱 키기
        
        if(GameManager.instance.nowStage == 1)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 1)
            {
                gameObject.transform.position = new Vector3(192, -23, 16);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 2)
            {
                gameObject.transform.position = new Vector3(192, -23, 18);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 3)
            {
                gameObject.transform.position = new Vector3(195, -23, 16);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 0)
            {
                gameObject.transform.position = new Vector3(195, -23, 18);
            }
            else
            {
                gameObject.transform.position = new Vector3(192, -23, 16);
            }
        }

        if (GameManager.instance.nowStage == 2)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 1)
            {
                gameObject.transform.position = new Vector3(3, 0, 27);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 2)
            {
                gameObject.transform.position = new Vector3(0, 0, 27);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 3)
            {
                gameObject.transform.position = new Vector3(-3, 0, 27);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 0)
            {
                gameObject.transform.position = new Vector3(-6, 0, 27);
            }
            else
            {
                gameObject.transform.position = new Vector3(3, 0, 27);
            }
        }

        if (GameManager.instance.nowStage == 3)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 1)
            {
                gameObject.transform.position = new Vector3(-30, 6, -32);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 2)
            {
                gameObject.transform.position = new Vector3(-32, 6, -32);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 3)
            {
                gameObject.transform.position = new Vector3(-34, 6, -32);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 0)
            {
                gameObject.transform.position = new Vector3(-36, 6, -32);
            }
            else
            {
                gameObject.transform.position = new Vector3(-30, 6, -32);
            }
        }
    }
    #endregion
}
