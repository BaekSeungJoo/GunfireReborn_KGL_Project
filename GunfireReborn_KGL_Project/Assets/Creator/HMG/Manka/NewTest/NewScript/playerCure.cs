using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class playerCure : MonoBehaviourPun
{
    public List<GameObject> otherPlayers;

    //회복바 Ui이미지
    public Image recoveryBar;
    //죽어가는속도
    public float dieSpeed;
    //회복속도
    public float recoverySpeed;
    //현재 회복진행도
    private float currentRecovery = 1.0f;
    //플레이어 상태를 가져오는 변수
    private playerHp hp;
    //회복중임을 체크하는 변수
    private bool isCure;
    //레이 변수
    public RaycastHit hitInfo;
    //레이를 쏘기시작할 캠
    public CinemachineVirtualCamera cam;
    //레이의 사정거리
    private float rayDistance = 5f;
    // Start is called before the first frame update
    void Start()
    {
        otherPlayers = new List<GameObject>();
        hp = GetComponent<playerHp>();
        cam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("GoingDead", RpcTarget.All);

        if (SearchPlayer() ==2)
        {//주변에 플레이어가 있을경우
         //여기서 레이캐스트를 발사해서 그로기 상태인 플레이어일경우 cureOther()를 사용하게만들음\
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, rayDistance))
            {   //레이가 충돌한경우
                Debug.DrawRay(cam.transform.position, cam.transform.forward * rayDistance, Color.blue);
                if (hitInfo.transform.CompareTag("Player")&&hitInfo.collider.gameObject.GetComponent<playerHp>().state == playerHp.State.groggy)
                {
                    if (Input.GetKey("Get"))
                    {
                        photonView.RPC("CureOther", RpcTarget.All);
                    }
                    else
                    {
                        // 회복 중단 시 회복 상태를 초기화합니다.
                        photonView.RPC("StopCure", RpcTarget.All);
                    }
                }
            }
            else
            {
                isCure = false;
            }
        }
    }

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

    //가까이있는 플레이어가 있는지 판별하는 함수
    private int SearchPlayer()
    {
        int peopleNum = 0;
        for(int i=0; i<otherPlayers.Count; i++)
        {
            if (otherPlayers[i] !=null)
            {
                peopleNum++;
            }
        }
        if(peopleNum ==0)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    [PunRPC]
    private  void CureOther()
    {
        isCure =true;
        currentRecovery += recoverySpeed * Time.deltaTime;
        recoveryBar.fillAmount = currentRecovery;
        if (currentRecovery >= 1.0f)
        {
            // 회복이 완료되면 부활 로직을 호출하거나 다른 필요한 작업을 수행합니다.
            hp.state = playerHp.State.play;
            hp.curHealth = hp.maxHealth / 2;
            isCure = false;
            currentRecovery = 1.0f;
        }
    }
    [PunRPC]
    private void GoingDead()
    {

        if (isCure == false)
        {
            currentRecovery -= dieSpeed * Time.deltaTime;
            recoveryBar.fillAmount = currentRecovery;
        }
    }

    [PunRPC]

    private void StopCure()
    {
        currentRecovery = 0.0f;
        recoveryBar.fillAmount = currentRecovery;
        isCure = false;
    }
}
