using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EvilMage : MeleeEnemyController
{
    public GameObject floorEffect;  // 바닥 이펙트
    public GameObject chargeEffect; // 공격 시작 이펙트
    public GameObject attackEffect; // 공격 시 이펙트
    public GameObject explosionEffect; // 폭탄 폭발 이벤트

    public Transform boomPos;       // 폭탄 생성 위치
    public GameObject boomPrefab;   // 폭탄 프리팹 (오브젝트 풀에서 꺼내서 사용할 것임)
    public GameObject fakeBoom;     // 연출을 위한 가짜 폭탄

    public Transform _TargetPlayer; // 타겟 플레이어

    private void Awake()
    {
        enemyType = Type.Range;
        
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        isIdle = true;              // 대기 상태
        isTracking = false;         // 추적 상태
        isAttacking = false;        // 공격 상태
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // 기본상태는 대기 상태
        if (isTracking == false && isAttacking == false)
        {
            isIdle = true;
            animator.SetBool("Idle", true);
        }

        // 플레이어와의 위치를 구해서 추적 거리안으로 다가오면 추적
        FindClosestPlayer();

        // 추적 대상이 있다면
        if (targetPlayer != null)
        {
            // 추적 플레이어를 바라본다. (브레스 쏠 때도 바라보게 하기 위함)
            // transform.LookAt(targetPlayer);

            // y값 회전 고정
            targetDirection = targetPlayer.position - transform.position;
            targetDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(targetDirection.normalized);

            if (isAttacking == false)
            {
                // 추적 플레이어를 향해서 이동한다.
                nav.SetDestination(targetPlayer.position);
            }

            // 공격 ( 추적 플레이어가 공격범위 안에 들어오면 )
            if (Vector3.Distance(transform.position, targetPlayer.position) <= attackRange)
            {
                StartAttack();
            }

            // 대기 ( 추적 범위 밖으로 플레이어가 나가면 )
            else if (Vector3.Distance(transform.position, targetPlayer.position) > trackingRange)
            {
                StartIdle();
            }
        }
    }

    // 폭탄 생성
    public void InitBoom()
    {
        // 기존 코드
        // GameObject boom = Instantiate(boomPrefab, boomPos.position, Quaternion.identity);
        // boom.transform.parent = transform;

        // 코드 변경
        // 폭탄의 생성 지점을 초기화한다.
        boomPrefab.transform.position = boomPos.transform.position;

        // 자식오브젝트인 폭탄을 활성화 해준다.
        boomPrefab.SetActive(true);

        // 부모 오브젝트와 같은 레벨로 올린다.
        boomPrefab.transform.SetParent(null);

    }

    // 가짜 폭탄 활성화
    public void InitFakeBoom()
    {
        fakeBoom.SetActive(true);
    }

    // 공격 모으기 ( 애니메이션 이벤트 )
    public void EvilMage_Charging()
    {
        // 브레스 모으기 활성화
        InitFakeBoom();
        floorEffect.SetActive(true);
        chargeEffect.SetActive(true);
    }


    // 공격 순간 (애니메이션 이벤트)
    public void EvilMage_Attack()
    {
        // Debug.Log("Attack");

        // 폭탄 생성
        InitBoom();

        // 공격 모으기 이펙트 비활성화
        chargeEffect.SetActive(false);
        // 바닥 이펙트 비활성화
        floorEffect.SetActive(false);

        // 공격 이펙트 활성화
        attackEffect.SetActive(true);
    }

    // 공격 끝 ( 애니메이션 이벤트 )
    public void EvilMage_AttackEnd()
    {
        // 공격 끝
        AttackEnd();

        // 공격 이펙트 비활성화
        attackEffect.SetActive(false);
    }

    // 폭발 이펙트 생성
    public void Active_BombEffect(Vector3 effectInitPos)
    {
        explosionEffect.transform.position = effectInitPos;
        explosionEffect.SetActive(true);
    }
}
