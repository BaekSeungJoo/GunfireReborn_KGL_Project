using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;

public class Boom : MonoBehaviourPun
{
    public Transform target_Tran;       // 타겟 위치
    public float initialAngle = 30f;    // 처음 날라가는 각도

    private EvilMage evilMage;          // 부모 클래스
    private Rigidbody rb;               // Rigidbody

    private void Awake()
    {
        evilMage = GetComponentInParent<EvilMage>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // 날아갈 타겟 플레이어의 transform
        target_Tran = evilMage.targetPlayer.transform;

        // 포물선 운동
        Vector3 velocity = GetVelocity(transform.position, target_Tran.position, initialAngle);      
        rb.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        photonView.RPC("InActiveBoom", RpcTarget.All);
    }

    public Vector3 GetVelocity(Vector3 startPos, Vector3 target, float initialAngle)
    {
        // Unity 현재 프로젝트의 물리적인 중력의 크기를 나타내는 값. 9.81
        float gravity = Physics.gravity.magnitude;

        // 처음 날아가는 각도를 라디안으로 변경
        float angle = initialAngle * Mathf.Deg2Rad;

        // 타겟 위치
        Vector3 targetPos = new Vector3(target.x, 0, target.z);

        // 처음 발사 위치
        Vector3 shotPos = new Vector3(startPos.x, 0, startPos.z);

        // 거리 구하기
        float distance = Vector3.Distance(targetPos, shotPos);

        // 높이 차이 계산 (초기 속도 계산 시 중력의 영향을 반영하기 위해)
        float yOffset = startPos.y - target.y;

        // { 발사체의 초기 속도를 계산하는 주요 공식
        // 거리의 제곱을 계산 : Mathf.Pow(distance, 2)
        // 거리 제곱에 중력의 절반을 곱한다. 이 값을 발사 각도에 대한 탄젠트 와 높이 차이 yOffset으로 나눔
        // 발사체의 발사 각도에 대한 코사인 : (1 / Mathf.Cos(angle))
        // Mathf.Cos(angle) : 의 역수를 곱한다. 이 부분은 발사 각도의 영향을 보정
        // Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
        
        float initialVelocity
            = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
        
        // } 발사체의 초기 속도를 계산하는 주요 공식


        // 초기 속도를 사용하여 3D 공간에서 발사체의 속도 벡터 계산
        // y 성분은 초기속도를 발사각도(라디안)의 사인 값으로 설정하여 발사체가 수직으로 올라갔다가 다시 아래로 떨어지는 곡선 운동을 나타냄
        // z 성분은 초기속도를 발사각도(라디안)의 코사인 값으로 설정하여 발사체가 수평 방향으로 이동하는데 사용됨.
        Vector3 velocity
            = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Vector3.Angle(Vector3.forward, planarTarget - planarPosition)
        // 이 부분은 현재 위치에서 목표 위치까지의 벡터와 전방 벡터 간의 각도를 계산
        // (target.x > player.x ? 1 : -1)
        // 이 부분은 목표 위치와 현재 위치의 x 좌표 값을 비교하여 목표가 현재 위치의 오른쪽에 있는지 왼쪽에 있는지 판단
        // 그에 따라 각도를 양수 또는 음수로 설정함. 이 값은 회전 방향을 결정하는데 사용
        float angleBetweenObjects
            = Vector3.Angle(Vector3.forward, targetPos - shotPos) * (target.x > startPos.x ? 1 : -1);


        // 초기 속도 벡터를 angleBetweenObjects각도만큼 Vector3.up 축 주위로 회전시키는 것을 나타냄
        // Quaternion.AngleAxis(angleBetweenObjects, Vector3.up)
        // 이 부분은 angleBetweenObjects 각도를 기준으로 Vector3.up 축 주위로 회전하는 쿼터니언을 생성
        // angleBetweenObjects는 앞서 계산된 현재 위치에서 목표 위치 까지의 각도를 나타내며, Vector3.up은 y축을 기준으로 회전을 나타냄
        // * velocity 초기속도벡터를 velocity에 적용하여, 초기 속도 벡터를 해당 각도 angleBetweenObjects 만큼 회전
        // 이렇게 회전 시킨 벡터가 최종속도가 된다.
        // 결과 적으로 finalVelocity에서는 초기 속도가 현재 위치에서 목표 위치까지의 각도angleBetweenObjects를 고려하여
        // 회전된 속도 벡터가 저장된다.
        // 이 속도 벡터를 사용하여 발사체가 곡선운동을 수행하도록 설정된다.

        Vector3 finalVelocity
            = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }

    [PunRPC]
    public void InActiveBoom()
    {
        // 이펙트 활성화
        evilMage.Active_BombEffect(gameObject.transform.position);

        // 부모 오브젝트 안으로 다시 들어온다
        gameObject.transform.SetParent(evilMage.boomPos);

        // 게임오브젝트(탄환) 비활성화
        this.gameObject.SetActive(false);
    }
}
