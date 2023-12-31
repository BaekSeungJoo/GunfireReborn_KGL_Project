using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.AI;

public class MeleeEnemyController : Enemy
{
    private void Awake()
    {
        enemyType = Type.Melee;

        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundEffect;

        isIdle = true;              // 대기 상태
        isTracking = false;         // 추적 상태
        isAttacking = false;        // 공격 상태
    }

    private void Update()
    {
        // 마스터 클라이언트가 아니라면 리턴
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // 기본상태는 대기 상태
        if (isTracking == false && isAttacking == false)
        {
            isIdle = true;
            // photonView.RPC("Melee_AnimSetBoolIdle", RpcTarget.All, true);

            animator.SetBool("Idle", true);
        }

        // 플레이어와의 위치를 구해서 추적 거리안으로 다가오면 추적
        FindClosestPlayer();

        // 추적 대상이 있다면
        if (targetPlayer != null)
        {
            if (isAttacking == false)
            {
                // 추적 대상 바라보기 ( y 값을 고정시켜서 높이차이가 있어도 이상하게 회전함을 방지 )
                targetDirection = targetPlayer.position - transform.position;
                targetDirection.y = 0;
                transform.rotation = Quaternion.LookRotation(targetDirection.normalized);

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

    [PunRPC]
    public void Melee_AnimSetBoolIdle(bool state)
    {
        animator.SetBool("Idle", state);
    }

    [PunRPC]
    public void Melee_AnimSetBoolTrack(bool state)
    {
        animator.SetBool("Track", state);
    }

    [PunRPC]
    public void Melee_AnimSetBoolAttack(bool state)
    {
        animator.SetBool("Attack", state);
    }


}
