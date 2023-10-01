using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SetDropVelocity : MonoBehaviourPun
{
    private Rigidbody rb;
    private float randonPosX;       // 생성될 때 튀어가나는 속도가 될 x값
    private float randonPosY;       // 생성될 때 튀어가나는 속도가 될 y값
    private float randonPosZ;       // 생성될 때 튀어가나는 속도가 될 z값
    private Vector3 newVelocity;    // 생성될 때 튀어나가는 속도

    private GameObject[] allPlayers;         // 모든 플레이어
    private GameObject myClientPlayer;       // 내 클라이언트의 플레이어
    private float moveSpeed = 5f;            // 플레이어 쪽으로 다가오는 속도
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // 플레이어 태그 중에 내 클라이언트의 플레이어를 찾고 위치 반환
        allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject myPlayer in allPlayers)
        {
            if (myPlayer.GetPhotonView().IsMine == true)
            {
                myClientPlayer = myPlayer;
            }
        }
    }

    private void OnEnable()
    {
        SetVelocity();
        rb.AddForce(newVelocity, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.CompareTag("weapon"))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void Update()
    {
        // 플레이어 쪽으로 이동 중인 경우
        if (myClientPlayer != null && this.gameObject.CompareTag("weapon") == false)
        {
            // 플레이어 쪽으로 이동
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, myClientPlayer.transform.position, step);
        }
    }



    public void SetVelocity()
    {
        randonPosX = Random.Range(0f, 0.1f);
        randonPosY = Random.Range(0f, 0.5f);
        randonPosZ = Random.Range(0f, 0.1f);
        newVelocity = new Vector3(randonPosX, randonPosY, randonPosZ);
    }
}
