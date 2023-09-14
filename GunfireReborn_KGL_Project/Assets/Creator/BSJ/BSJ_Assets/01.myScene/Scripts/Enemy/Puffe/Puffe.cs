using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.AI;

// 몬스터 Puffe 클래스
public class Puffe : Enemy
{
    public int damage;

    public GameObject gatherEnergyVFX;      // 이펙트 1
    public GameObject gatherEnergyVFX2;     // 이펙트 2
    public GameObject energyShotVFX;        // 이펙트 3
    public GameObject magicCircleVFX;       // 이펙트 4

    public LineRenderer lineRenderer;       // 라인 렌더러
    public GameObject shotPoint;            // 라인 렌더러 시작 위치
    public Material lineRenMat;             // 라인 렌더러 머테리얼

    private Vector3 startPoint;     // 레이 시작 지점
    private Vector3 direction;      // 레이 방향
    private Vector3 endPoint;       // 레이 끝 지점

    private Vector3 targetDirection;

    private void Awake()
    {
        enemyType = Type.Range;

        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        isIdle = true;              // 대기 상태
        isTracking = false;         // 추적 상태
        isAttacking = false;        // 공격 상태

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
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
            if (isAttacking == false)
            {
                // 추적 플레이어를 바라본다.
                // transform.LookAt(targetPlayer);

                // y값 회전 고정
                targetDirection = targetPlayer.position - transform.position;
                targetDirection.y = 0;
                transform.rotation = Quaternion.LookRotation(targetDirection.normalized);

                // 추적 플레이어를 향해서 이동한다.
                nav.SetDestination(targetPlayer.position);
            }

            // 공격 ( 추적 플레이어가 공격범위 안에 들어오면 )
            if (Vector3.Distance(transform.position, targetPlayer.position) <= attackRange)
            {
                // 공격 시작
                StartAttack();
            }

            // 대기 ( 추적 범위 밖으로 플레이어가 나가면 )
            else if (Vector3.Distance(transform.position, targetPlayer.position) > trackingRange)
            {
                // 대기 시작
                StartIdle();
            }
        }
    }

    // 애니메이션 이벤트
    public void StartEnergyShot()
    {
        // 이미션 색상 노랑으로 변경
        ChangeEmissionColor(Color.yellow);

        // 추적 플레이어를 바라본다.
        transform.LookAt(targetPlayer);

        // 이펙트 활성화
        gatherEnergyVFX.gameObject.SetActive(true);
        gatherEnergyVFX2.gameObject.SetActive(true);
        magicCircleVFX.gameObject.SetActive(true);

        // Line Renderer 위치 설정
        // 시작점
        startPoint = shotPoint.transform.position;
        // 방향
        direction = (targetPlayer.position - startPoint).normalized;
        // 끝점
        endPoint = startPoint + direction * 50f;

        // Line Renderer 시작점과 끝점 설정
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);

        // Line Renderer 켜기
        lineRenderer.enabled = true;
    }

    public void EnergyShot()
    {
        // 이미션 색상 빨강으로 변경
        ChangeEmissionColor(Color.red);

        // 이펙트
        gatherEnergyVFX.gameObject.SetActive(false);
        gatherEnergyVFX2.gameObject.SetActive(false);

        energyShotVFX.gameObject.SetActive(true);

        // 레이 발사 (라인 렌더 시작점 ~ 라인 렌더 끝점)
        RaycastHit hit;
        if(Physics.Raycast(startPoint, direction, out hit, Vector3.Distance(startPoint, endPoint)))
        {
            Debug.DrawRay(startPoint, direction, Color.white);
            // 충돌한 물체가 있다면
            if(hit.collider != null && hit.collider.CompareTag("Player"))
            {
                // Health 스크립트에 있는 TakeDamage 메서드 RPC (remote procedure call)
                // hit.transform.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
    }

    public void EndEnergyShot()
    {
        // Line Renderer 끄기
        lineRenderer.enabled = false;

        // 이펙트
        energyShotVFX.gameObject.SetActive(false);
        magicCircleVFX.gameObject.SetActive(false);
    }

    // 이미션 색상 변경
    public void ChangeEmissionColor(Color color)
    {
        // 머티리얼의 에미션 색상을 변경합니다.
        lineRenMat.SetColor("_EmissionColor", color);

        // 머티리얼의 에미션 활성화를 활성화해야 색상 변경이 표시됩니다.
        lineRenMat.EnableKeyword("_EMISSION");
    }
}
