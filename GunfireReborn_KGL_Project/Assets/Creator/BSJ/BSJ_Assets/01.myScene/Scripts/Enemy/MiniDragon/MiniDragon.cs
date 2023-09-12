using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniDragon : MeleeEnemyController
{
    public GameObject attackHitBox; // 공격 범위
    public GameObject floorEffect;  // 바닥 이펙트
    public GameObject chargeEffect; // 공격 시작 이펙트

    private void Awake()
    {
        enemyType = Type.Melee;

        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        isIdle = true;              // 대기 상태
        isTracking = false;         // 추적 상태
        isAttacking = false;        // 공격 상태
    }

    private void Update()
    {
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

    // 브레스 모으기 ( 애니메이션 이벤트 )
    public void chargeBreath()
    {
        // 브레스 모으기 활성화
        chargeEffect.gameObject.SetActive(true);
    }


    // 공격 순간 (애니메이션 이벤트)
    public void MiniDragonAttack()
    {
        Debug.Log("Attack");

        // 브레스 모으기 이펙트 비활성화
        chargeEffect.gameObject.SetActive(false);

        // 공격 범위 활성화
        attackHitBox.gameObject.SetActive(true);

        // 바닥 이펙트 활성화
        floorEffect.gameObject.SetActive(true);
    }

    // 공격 끝 ( 애니메이션 이벤트 )
    public void MiniDragonAttackEnd()
    {
        Debug.Log("AttackEnd");

        isAttacking = false;
        animator.SetBool("Attack", false);
        targetPlayer = null;

        // 공격 범위 비활성화
        attackHitBox.gameObject.SetActive(false);

        // 바닥 이펙트 비활성화
        floorEffect.gameObject.SetActive(false);
    }
}
