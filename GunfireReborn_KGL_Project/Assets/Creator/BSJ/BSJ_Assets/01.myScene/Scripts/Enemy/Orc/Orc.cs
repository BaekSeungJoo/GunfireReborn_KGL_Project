using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Orc : Enemy
{
    public GameObject Attack01_chargeEffect;    // 패턴 공격 1 공격 차지 이펙트
    public GameObject Attack01_Effect01;        // 패턴 공격 1 공격 시 이펙트 1
    public GameObject Attack01_Effect02;        // 패턴 공격 1 공격 시 이펙트 2

    public GameObject StartAttack02_Effect;     // 패턴 공격 2 공격 예열 이펙트
    public GameObject Attack02_Effect01;        // 패턴 공격 2 공격 시 이펙트 1
    public GameObject Attack02End_Effect;       // 패턴 공격 2 공격 끝 이펙트
    public GameObject AttackEndStun_Effect;     // 패턴 공격 2 스턴 이펙트

    public bool isPattern01;    // 공격 패턴 1
    public bool isPattern02;    // 공격 패턴 2

    public bool isStop;         // 정지 중인지 판단

    public int ranPattern = 0;      // 어떤 패턴을 실행할 것인가?

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
        isPattern01 = false;        // 패턴 1
        isPattern02 = false;        // 패턴 2
        isStop = false;             // 정지 상태
    }

    private void Start()
    {
        // 처음 패턴 정하기
        SetNextPattern();
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
            // 추적 대상 바라보기 ( y 값을 고정시켜서 높이차이가 있어도 이상하게 회전함을 방지 )
            targetDirection = targetPlayer.position - transform.position;
            targetDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(targetDirection.normalized);

            if (isAttacking == false)
            {
                // 추적 플레이어를 향해서 이동한다.
                nav.SetDestination(targetPlayer.position);
            }
            
            if(isAttacking == true && isStop == false)
            {
                // 추적 플레이어를 향해서 이동한다.
                nav.SetDestination(targetPlayer.position);
            }

            // 공격 ( 추적 플레이어가 공격범위 안에 들어오면 )
            if (Vector3.Distance(transform.position, targetPlayer.position) <= attackRange)
            {

                //// 패턴 01
                if (ranPattern == 0)
                {
                    isPattern01 = true;
                    isPattern02 = false;
                    animator.SetBool("Pattern_01", true);
                    animator.SetBool("Pattern_02", false);

                    StartAttack();
                }

                // 패턴 02
                else if (ranPattern == 1)
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

    #region 패턴 1
    // 패턴 1 공격 차지 ( 애니메이션 이벤트 )
    public void Orc_Pattern01_ChargeEffect()
    {
        // 이펙트 활성화
        Attack01_chargeEffect.SetActive(true);
    }

    // 패턴 1 공격 차지 끝 ( 애니메이션 이벤트 )
    public void Orc_Pattern01_ChargeEffectEnd()
    {
        // 이펙트 활성화
        Attack01_chargeEffect.SetActive(false);
    }


    // 패턴 1 공격하는 순간 ( 애니메이션 이벤트 )
    public void Orc_Pattern01_Attack01()
    {
        // 이펙트
        Attack01_Effect01.SetActive(true);
        Attack01_Effect02.SetActive(true);

        // 근접 (트리거를 가진 자식오브젝트 활성화해서 플레이어에게 데미지를 주는 방식)
        if (enemyType == Type.Melee)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    // 패턴 1 공격 끝 ( 애니메이션 이벤트 )
    public void Orc_Pattern01_EndAttack()
    {
        // 이펙트
        Attack01_Effect01.SetActive(false);
        Attack01_Effect02.SetActive(false);
        Attack02_Effect01.SetActive(false);

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
    }
    #endregion

    #region 패턴 2
    // 패턴 2 공격 예열 이펙트 ( 애니메이션 이펙트 )
    public void Orc_Pattern02_StartAttckEffect()
    {
        // 정지
        Stop();

        // 이펙트
        StartAttack02_Effect.SetActive(true);
    }

    // 패턴 2 공격하는 순간 ( 애니메이션 이벤트 )
    public void Orc_Pattern02_Attack()
    {
        // 정지 해제
        isStop = false;

        // 이펙트
        StartAttack02_Effect.SetActive(false);
        Attack02_Effect01.SetActive(true);

        // 근접 (트리거를 가진 자식오브젝트 활성화해서 플레이어에게 데미지를 주는 방식)
        if (enemyType == Type.Melee)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    // 패턴 2 공격 종료 ( 애니메이션 이벤트 )
    public void Orc_Pattern02_EndAttack()
    {
        // 이펙트
        Attack01_Effect01.SetActive(false);
        Attack01_Effect02.SetActive(false);
        Attack02_Effect01.SetActive(false);
        Attack02End_Effect.SetActive(true);
        AttackEndStun_Effect.SetActive(true);


        // 근접 (트리거를 가진 자식오브젝트 활성화해서 플레이어에게 데미지를 주는 방식)
        if (enemyType == Type.Melee)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    // 패턴 2 스턴 끝 ( 애니메이션 이벤트 )
    public void Orc_Pattern02_EndStun()
    {
        Attack02End_Effect.SetActive(false);
        AttackEndStun_Effect.SetActive(false);
        isStop = false;
    }


    // 패턴 2 최종 패턴 종료 ( 애니메이션 이벤트 )
    public void Orc_Pattern02_FinalEnd()
    {
        isAttacking = false;
        isPattern01 = false;
        isPattern02 = false;

        animator.SetBool("Attack", false);
        animator.SetBool("Track", false);
        animator.SetBool("Pattern_01", false);
        animator.SetBool("Pattern_02", false);

        animator.SetBool("Idle", true);
        targetPlayer = null;
    }
    #endregion

    // 다음 패턴 정하는 함수
    public void SetNextPattern()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        int randomPatternNumber = Random.Range(0, 2);

        photonView.RPC("Pun_SetNextPattern", RpcTarget.All, randomPatternNumber);
    }

    [PunRPC]
    public void Pun_SetNextPattern(int patternNum)
    {
        Debug.Log(patternNum);
        // 다음 패턴 정하기
        ranPattern = patternNum;
    }


    // 정지하는 함수
    public void Stop()
    {
        isStop = true;
        nav.ResetPath();
        nav.isStopped = true;
        nav.velocity = Vector3.zero;
    }
}
