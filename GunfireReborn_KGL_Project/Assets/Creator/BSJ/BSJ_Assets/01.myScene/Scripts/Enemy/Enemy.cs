using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

// 부모 클래스
public class Enemy : MonoBehaviour
{
    public enum Type { Melee, Range, Boss}; // 타입 ( 근거리, 원거리, 보스)
    public Type enemyType;                  // 적 타입

    public float trackingSpeed;             // 추적 스피드
    public float trackingRange;             // 추적 거리
    public float attackRange;               // 공격 시작 범위

    public Transform targetPlayer;          // 플레이어 위치
    public PhotonView trackPlayer;           // 최종 추적할 플레이어
    public Animator animator;               // 애니메이터

    public bool isIdle;                     // 대기 상태
    public bool isTracking;                 // 추적 상태
    public bool isAttacking;                // 공격 상태

    public NavMeshAgent nav;                // 네비게이션 사용

    private void Awake()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    // 제일 가까운 플레이어 찾기
    public void FindClosestPlayer()
    {
        // 포톤뷰 스크립트를 갖고 있는 대상 탐색
        PhotonView[] players = GameObject.FindObjectsOfType<PhotonView>();

        // 추적 처음 거리는 무한대로 설정
        float closestDistance = Mathf.Infinity;

        // photonview를 갖고 있는 모든 대상 중에 Player 태그를 가진 대상 탐색
        foreach (PhotonView player in players)
        {
            if (player.CompareTag("Player"))
            {
                // 탐색 대상과의 거리 구하기
                float distance = Vector3.Distance(transform.position, player.transform.position);

                // 거리가 closestDistance보다 가까우면 제일 가까운 대상 새로 저장
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    trackPlayer = player;
                }
            }
        }

        // 위에서 구한 closestDistance값이 추적 거리보다 작다면,
        if (closestDistance <= trackingRange)
        {
            // 추적 시작
            nav.isStopped = false;
            isTracking = true;
            isIdle = false;
            animator.SetBool("Track", true);
            animator.SetBool("Idle", false);

            targetPlayer = trackPlayer.transform;
            // Debug.Log(closestDistance);
        }
    }

    // 공격 시작
    public void StartAttack()
    {
        // 정지한 상태로 공격모션만 시행
        // rb.velocity = Vector3.zero;
        nav.isStopped = true;
        nav.velocity = Vector3.zero;

        isAttacking = true;
        isTracking = false;
        animator.SetBool("Attack", true);
        animator.SetBool("Track", false);
    }

    // 공격하는 순간 ( 애니메이션 이벤트 )
    public void Attack()
    {
        nav.isStopped = true;
        nav.velocity = Vector3.zero;

        Debug.Log("Attack");

        // 근접 (트리거를 가진 자식오브젝트 활성화해서 플레이어에게 데미지를 주는 방식)
        if(enemyType == Type.Melee)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    // 공격 끝 ( 애니메이션 이벤트 )
    public void AttackEnd()
    {
        nav.isStopped = true;
        nav.velocity = Vector3.zero;

        Debug.Log("AttackEnd");
        // 근접 (트리거를 가진 자식오브젝트 활성화해서 플레이어에게 데미지를 주는 방식)
        if(enemyType == Type.Melee)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        
        isAttacking = false;
        animator.SetBool("Attack", false);
        targetPlayer = null;
    }



    // 대기 시작
    public void StartIdle()
    {
        nav.isStopped = true;
        nav.velocity = Vector3.zero;

        isTracking = false;
        animator.SetBool("Track", false);

        targetPlayer = null;
    }
}
