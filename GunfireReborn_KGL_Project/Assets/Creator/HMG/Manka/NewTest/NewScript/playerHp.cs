
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
    private int curHealth;       //현재HP
    private int maxShield;       //최대shield;
    private int curShield;       //현재shield;

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

    playerHp otherHP;

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
        hpBar = GameObject.Find("HPBar").GetComponent<Image>();              //hp바 받아오기
        shieldBar = GameObject.Find("ShieldBar").GetComponent<Image>();      //shield바 받아오기
        virtualCam = FindObjectOfType<CinemachineVirtualCamera>();              //버츄얼캠 받아오기
        ik = GetComponent<IK1>();                                               //3인칭ik 받아오기
        animator = GetComponent<Animator>();                                    //내 애니메이터 받아오기
        FPSUnityChan = Camera.main.transform.GetChild(0).gameObject;               //1인칭ik 받아오기

        maxHealth = 100;
        maxShield = 100;
        state = State.play;                                                     //현재 상태를 play로 해놓음
        curHealth = maxHealth;                                                  //현재HP를 MAXHP로 초기화
        curShield = maxShield;                                                  //현재쉴드를 MAXShield로 초기화
        hpBar.fillAmount = (float)curHealth / (float)maxHealth;                      //hp바의 초기화
        shieldBar.fillAmount = (float)curShield / (float)maxShield;                  //Shield바의 초기화

        recoveryBar.fillAmount = 1;                                             //리커버리바 1로 초기화
        isturnGroggy = true;
        isturnPlay = true;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (state == State.groggy)
        {   //그로기 상태에서는 죽어가는함수를 사용한다.
            photonView.RPC("GoingDead", RpcTarget.All);
        }
        //RPC로 전해줘야하는부분
        StateUpdate();     //상태를 업데이트하는 함수,hp,shield,애니메이터파라미터,
        //RPC로 전해주지않아도된는, 전해주면안되는 부분
        if (state == State.groggy)                              // 그로기상태라면 
        {
            animator.SetBool("groggy", true);                   // 애니메이터 파라미터를 groggy로 만들어서 groggy애니메이션이 재생되게만들음
        }
        if (state == State.play)                                                   // 플레이상태라면
        {
            animator.SetBool("groggy", false);                  // 애니메이터 파라미터를 groggy로 만들어서 groggy애니메이션이 재생되지않게만들음
            if(isturnPlay == false)
            {
                TurnPlay();
            }
            ShotRayCast();                                      // 플레이중이라면 rayCast를 쏘게만들음
        }
        if (state == State.die)                                 // 죽은상태라면 
        {
            if (isDead == false)
            {
                animator.SetTrigger("Dead");                        // 죽음 애니메이션을 재생되게함
                
                isDead = true;
            }
            ik.enabled = false;
        }
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(recoveryBar);
    //        stream.SendNext(state);
    //    }
    //    else
    //    {
    //        recoveryBar.fillAmount = (float)stream.ReceiveNext();
    //        state = (State)stream.ReceiveNext();
    //    }
    //}

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

            photonView.RPC("PlayerHealthUpdated", RpcTarget.All, curHealth, curShield);
           
        }
        //쉴드가 남아있다면 쉴드가 까이게 하고 쉴드가 0이거나 이하라면 hp가 까이게함
        //현재 hp가 0보다 낮게된다면 플레이어를 그로기상태로만들음
       
        //포톤네트워크마스터 클라이언트로 쉴드와 hp를 다른월드의 내게 적용시킴
    }
    #endregion
    [PunRPC]
    private void TurnStateGroggy()
    {
        state = State.groggy;
    }


    [PunRPC]
    private void PlayerHealthUpdated(int newCurHealth, int newCurShield)
    {
        curHealth = newCurHealth;
        curShield = newCurShield;

        if (curHealth <= 0)
        {
            photonView.RPC("TurnStateGroggy", RpcTarget.All);
            isturnGroggy = false;
            isturnPlay = false;
            Debug.Log("BeforeTurn");
            TurnGroggy();
            Debug.Log("AfterTurn");
            photonView.RPC("TagChangeGroggy", RpcTarget.All);
        }
    }

    #region 상태업데이트함수
    private void StateUpdate()
    {

        shieldBar.fillAmount = (float)curShield / (float)maxShield;
        hpBar.fillAmount = (float)curHealth / (float)maxHealth;

        if (state == State.groggy)                              // 그로기상태라면 
        {
            roation.enabled = false;                            // 플레이어의 로테이션을 꺼버림
            photonView.RPC("MakeTrueRecoveryBar", RpcTarget.All);                // 큐어 바를 활성화시킴
        }
        else                                                    // 그로기상태가 아니라면
        {
            roation.enabled = true;                             // 플레이어의 로테이션을 켜버림
            photonView.RPC("MakeFalseRecoveryBar", RpcTarget.All);               // 큐어 바를 활성화시킴
        }
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
    [PunRPC]
    private void TagChangeGroggy()
    {
        gameObject.tag = "Groggy";
    }
    
    #region 그로기상태로 변하는 함수와, 플레이상태로 변하는 함수
    private void TurnGroggy()
    {
        Debug.Log("Turn1");
        if(!photonView.IsMine)
        {
            return;
        }
        Debug.Log("Turn2");
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

        photonView.RPC("FalseWeapons", RpcTarget.All);      // 3인칭 무기 모두 false로 만들기
        ik.enabled = false;                                 // 3인칭 ik false로 만들기
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
        
        isturnPlay = true;
    }
    #endregion
    #region 죽어가는함수
    [PunRPC]
    private void GoingDead()
    {
        if (isCure == false)
        {
            // 10초 동안 감소해야 할 양을 계산합니다.
            float decreaseAmount = 1.0f / dyingTime * dieSpeed * Time.deltaTime;

            
            // fillAmount를 업데이트합니다.
            recoveryBar.fillAmount = Mathf.Max(recoveryBar.fillAmount - decreaseAmount, 0);
            // fillAmount가 0이면 필요한 처리를 수행합니다.
            if (recoveryBar.fillAmount == 0)
            {
                //죽음으로 처리함.
                state = State.die;
            }
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
        if (Physics.Raycast(virtualCam.transform.position, virtualCam.transform.forward, out hitInfo, rayDistance))
        {   //레이가 충돌한경우 hitInfo에 충돌 정보를 받아온다.

            //Debug.LogFormat("레이 물체 : {0}" , hitInfo.collider.gameObject.name);
            Debug.DrawRay(virtualCam.transform.position, virtualCam.transform.forward * rayDistance, Color.black);
            //playerHp test = hPlayer.GetComponent<playerHp>();
            if (hitInfo.transform.CompareTag("Groggy"))
            {
                //맞은물체의 Tag가 그로기고(플레이어일시 몬스터가 계속 때려서 흔들림) 맞은 플레이어가 그로기 상태라면
                if (Input.GetButton("Get"))
                {   //f를누르고있으면
                    hPlayer = hitInfo.collider.gameObject;
                    Debug.Log("클론의 태그 : " + hPlayer.tag);
                    //지금 레이를 맞은 플레이어를 회복하고있는 플레이어 변수에 저장한다.

                    hPlayer.GetComponent<playerHp>().photonView.RPC("Cure", RpcTarget.All);
                    //cureOther함수를 실행한다.
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
    #endregion


    [PunRPC]
    private void MakeTrueRecoveryBar()
    {
        recoveryBarOB.SetActive(true);
        ik.enabled = false;
    }

    [PunRPC]
    private void MakeFalseRecoveryBar()
    {
        recoveryBarOB.SetActive(false);
        ik.enabled = true;
    }
    [PunRPC]
    private void Cure()
    {
        // 현재 치유중 여부를 나타내는 변수를 true로 만들음 
        if (PhotonNetwork.IsMasterClient)
        {
            otherHP = GetComponent<playerHp>();
            isCure = true;
            // 5초 동안 증가해야 할 양을 계산합니다.
            float increaseAmount = 1.0f / cureTime * recoverySpeed * Time.deltaTime;
            // fillAmount를 업데이트합니다.
            otherHP.recoveryBar.fillAmount = Mathf.Max(recoveryBar.fillAmount + increaseAmount, 0);
            photonView.RPC("UpdateRecoveryBar", RpcTarget.All,otherHP.recoveryBar.fillAmount);
            if (otherHP.recoveryBar.fillAmount >= 1.0f)
            {
                // 플레이어의 스테이트가 play가되면 playerCure이 꺼지므로 반드시 제일 마지막에 두어야한다.
                photonView.RPC("ChangeTagState", RpcTarget.All);
                photonView.RPC("TrueWeapons",RpcTarget.All);
                isCure = false;
            }
        }
    }
    [PunRPC]
    private void UpdateRecoveryBar(float fillAmount)
    {
        recoveryBar.fillAmount = fillAmount;
    }

    [PunRPC]

    private void ChangeTagState()
    {
        gameObject.tag = "Player";
        otherHP.state = State.play;
        gameObject.GetComponent<Animator>().SetBool("groggy", false);
    }

    [PunRPC]
    private void TrueWeapons()
    {
        weapon.GetChild(2).gameObject.SetActive(true);
        FPSUnityChan.SetActive(true);                       // 1인칭 유니티짱 키기
    }
}
