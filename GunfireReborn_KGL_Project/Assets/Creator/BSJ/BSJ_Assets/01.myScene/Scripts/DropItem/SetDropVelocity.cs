using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SetDropVelocity : MonoBehaviour
{
    private Rigidbody rb;
    private float randonPosX;       // 생성될 때 튀어가나는 속도가 될 x값
    private float randonPosY;       // 생성될 때 튀어가나는 속도가 될 y값
    private float randonPosZ;       // 생성될 때 튀어가나는 속도가 될 z값
    private Vector3 newVelocity;    // 생성될 때 튀어나가는 속도

    private Transform playerTransform;  // 플레이어의 위치
    private float moveSpeed = 5f;            // 플레이어 쪽으로 다가오는 속도
    private float activeFalseDistance = 2f;     // 해당 거리만큼 가까이가면 비활성화
    private bool isMoveTowardPlayer = false;    // 플레이어 이동 중인지 체크
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        SetVelocity();
        rb.AddForce(newVelocity, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Ground") && gameObject.transform.CompareTag("weapon"))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isMoveTowardPlayer = true;
        }
    }

    private void Update()
    {
        // 플레이어 쪽으로 이동 중인 경우
        if(isMoveTowardPlayer && playerTransform != null)
        {
            // 플레이어 쪽으로 이동
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

            // 일정거리 이상 들어가면 비활성화
            if(Vector3.Distance(playerTransform.position, transform.position) < activeFalseDistance)
            {
                gameObject.SetActive(false);

                // 플레이어 쪽에서 해당 아이템을 먹었을 때 실행하는 로직
            }
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
