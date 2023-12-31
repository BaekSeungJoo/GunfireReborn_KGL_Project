using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.AI;

public class Creature1 : Enemy
{
    public int ranPattern;      // 어떤 패턴을 실행할 것인가?

    public bool isPattern01;    // 공격 패턴 1
    public bool isPattern02;    // 공격 패턴 2

    public AudioClip pattern01AttackAudio;
    public AudioClip pattern02AttackAudio;

    public GameObject attack01chargeEffect; // 공격 차지 이펙트
    public GameObject attack01Effect;
    public GameObject attack02Effect;

    private void Awake()
    {
        enemyType = Type.Melee;

        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        isIdle = true;              // 대기 상태
        isTracking = false;         // 추적 상태
        isAttacking = false;        // 공격 상태
        isPattern01 = false;        // 패턴 1
        isPattern02 = false;        // 패턴 2
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
            ranPattern = Random.Range(0, 2);    // 패턴 정하기
        }

        // 플레이어와의 위치를 구해서 추적 거리안으로 다가오면 추적
        FindClosestPlayer();

        // 추적 대상이 있다면
        if (targetPlayer != null)
        {
            // 추적 대상 바라보기 ( y 값을 고정시켜서 높이차이가 있어도 이상하게 회전함을 방지 )
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
                // 패턴 01
                if (ranPattern == 0)
                {
                    isPattern01 = true;
                    isPattern02 = false;
                    animator.SetBool("Pattern_01", true);
                    animator.SetBool("Pattern_02", false);

                    StartAttack();
                }

                // 패턴 02
                else if(ranPattern == 1)
                {
                    isPattern01 = false;
                    isPattern02 = true;
                    animator.SetBool("Pattern_01", false);
                    animator.SetBool("Pattern_02", true);

                    StartAttack();
                }
            }

            // 대기 ( 추적 범위 밖으로 플레이어가 나가면 )
            else if (Vector3.Distance(transform.position, targetPlayer.position) > trackingRange)
            {
                StartIdle();
            }
        }
    }

    // 패턴 1 공격 차지 ( 애니메이션 이벤트 )
    public void ChargeEffect_Creature1()
    {
        // 이펙트 활성화
        attack01chargeEffect.SetActive(true);
    }

    // 패턴 1 공격 차지 끝 ( 애니메이션 이벤트 )
    public void ChargeEffectEnd_Creature1()
    {
        // 이펙트 활성화
        attack01chargeEffect.SetActive(false);
    }


    // 패턴 1 공격하는 순간 ( 애니메이션 이벤트 )
    public void Attack01_Creature1()
    {
        // 근접 (트리거를 가진 자식오브젝트 활성화해서 플레이어에게 데미지를 주는 방식)
        if (enemyType == Type.Melee)
        {
            transform.GetChild(0).gameObject.SetActive(true);

            attack01Effect.SetActive(true);
            audioSource.clip = pattern01AttackAudio;
            audioSource.Play();
        }
    }

    // 패턴 2 공격하는 순간 ( 애니메이션 이벤트 )

    public void Attack02_Creature1()
    {
        // 근접 (트리거를 가진 자식오브젝트 활성화해서 플레이어에게 데미지를 주는 방식)
        if (enemyType == Type.Melee)
        {
            transform.GetChild(1).gameObject.SetActive(true);

            attack02Effect.SetActive(true);
            audioSource.clip = pattern02AttackAudio;
            audioSource.Play();
        }
    }

    // 공격 끝 ( 애니메이션 이벤트 )
    public void AttackEnd_Creature1()
    {
        // Debug.Log("AttackEnd");

        attack01Effect.SetActive(false);
        attack02Effect.SetActive(false);

        // 근접 (트리거를 가진 자식오브젝트 활성화해서 플레이어에게 데미지를 주는 방식)
        if (enemyType == Type.Melee)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }

        isAttacking = false;
        isPattern01 = false;
        isPattern02 = false;
        animator.SetBool("Attack", false);
        animator.SetBool("Track", false);
        animator.SetBool("Pattern_01", false);
        animator.SetBool("Pattern_02", false);

        animator.SetBool("Idle", true);
        targetPlayer = null;

        // 다음 패턴 정하기
        ranPattern = Random.Range(0, 2);
    }
}
