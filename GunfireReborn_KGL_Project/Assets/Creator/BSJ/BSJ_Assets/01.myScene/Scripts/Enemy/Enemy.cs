using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System.Threading;
using Photon.Realtime;

// 부모 클래스
public class Enemy : MonoBehaviourPun //IPunObservable
{
    public enum Type { Melee, Range, Boss}; // 타입 ( 근거리, 원거리, 보스)
    public Type enemyType;                  // 적 타입

    public float trackingSpeed;             // 추적 스피드
    public float trackingRange;             // 추적 거리
    public float attackRange;               // 공격 시작 범위

    public Transform targetPlayer;          // 플레이어 위치
    public PhotonView trackPlayer;       // 최종 추적할 플레이어 ( 포톤 )
    //public GameObject trackPlayer;          // 최종 주적할 플레이어
    public Animator animator;               // 애니메이터

    public bool isIdle;                     // 대기 상태
    public bool isTracking;                 // 추적 상태
    public bool isAttacking;                // 공격 상태

    public NavMeshAgent nav;                // 네비게이션 사용

    protected Vector3 targetDirection;      // 추적 방향

    // 오디오
    public AudioClip soundEffect;
    public AudioSource audioSource;

    // 기본 공격 이펙트
    public GameObject normalAttackEffect;
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        stream.SendNext(isIdle);
    //        stream.SendNext(isTracking);
    //        stream.SendNext(isAttacking);
    //    }
    //    else
    //    {
    //        isIdle = (bool)stream.ReceiveNext();
    //        isTracking = (bool)stream.ReceiveNext();
    //        isAttacking = (bool)stream.ReceiveNext();
    //    }
    //}

    private void Awake()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundEffect;
    }

    // 제일 가까운 플레이어 찾기
    public void FindClosestPlayer()
    {
        // 포톤뷰 스크립트를 갖고 있는 대상 탐색
        // PhotonView[] players = GameObject.FindObjectsOfType<PhotonView>();
        // 테스트 용
        PhotonView[] players = PhotonView.FindObjectsOfType<PhotonView>();
        List<PhotonView> playerWithTag = new List<PhotonView>();

        foreach (PhotonView player in players)
        {
            if (player.gameObject.CompareTag("Player"))
            {
                playerWithTag.Add(player);
            }
        }

        // 추적 처음 거리는 무한대로 설정
        float closestDistance = Mathf.Infinity;

        // photonview를 갖고 있는 모든 대상 중에 Player 태그를 가진 대상 탐색
        // foreach (PhotonView player in players)
        // 테스트
        // foreach (GameObject player in players)
        foreach (PhotonView player in playerWithTag)
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
                    photonView.RPC("SetTrackPlayer", RpcTarget.Others, trackPlayer.ViewID);
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

            // photonView.RPC("AnimSetBoolTrack", RpcTarget.All, true);
            // photonView.RPC("AnimSetBoolIdle", RpcTarget.All, false);

            animator.SetBool("Track", true);
            animator.SetBool("Idle", false);

            targetPlayer = trackPlayer.transform;
            photonView.RPC("SetTargetPlayer", RpcTarget.Others);
            // Debug.Log(closestDistance);
        }
    }

    // 공격 시작
    public void StartAttack()
    {
        // 정지한 상태로 공격모션만 시행
        nav.ResetPath();

        nav.isStopped = true;
        nav.velocity = Vector3.zero;

        isAttacking = true;
        isTracking = false;

        // photonView.RPC("AnimSetBoolAttack", RpcTarget.All, true);
        // photonView.RPC("AnimSetBoolTrack", RpcTarget.All, false);
        animator.SetBool("Attack", true);
        animator.SetBool("Track", false);
    }

    // 공격하는 순간 ( 애니메이션 이벤트 )
    public void Attack()
    {
        // Debug.Log("Attack");

        // 근접 (트리거를 가진 자식오브젝트 활성화해서 플레이어에게 데미지를 주는 방식)
        if(enemyType == Type.Melee)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    // 공격 끝 ( 애니메이션 이벤트 )
    public void AttackEnd()
    {
        // Debug.Log("AttackEnd");
        // 근접 (트리거를 가진 자식오브젝트 활성화해서 플레이어에게 데미지를 주는 방식)
        if(enemyType == Type.Melee)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        
        isAttacking = false;
        //photonView.RPC("AnimSetBoolAttack", RpcTarget.AllBuffered, animator, false);
        animator.SetBool("Attack", false);

        targetPlayer = null;

        if(normalAttackEffect != null)
        {
            normalAttackEffect.SetActive(false);
        }
    }



    // 대기 시작
    public void StartIdle()
    {
        nav.isStopped = true;
        nav.velocity = Vector3.zero;

        isTracking = false;
        //photonView.RPC("AnimSetBoolTrack", RpcTarget.All, false);
        animator.SetBool("Track", false);

        targetPlayer = null;
    }

    // 공격 이펙트, 효과음
    public void Attack_VFX()
    {
        if(normalAttackEffect != null)
        {
            normalAttackEffect.SetActive(true);
        }

        audioSource.Play();
    }

    [PunRPC]
    public void AnimSetBoolIdle(bool state)
    {
        animator.SetBool("Idle", state);
    }

    [PunRPC]
    public void AnimSetBoolTrack(bool state)
    {
        animator.SetBool("Track", state);
    }

    [PunRPC]
    public void AnimSetBoolAttack(bool state)
    {
        animator.SetBool("Attack", state);
    }

    [PunRPC]
    public void SetTrackPlayer(int viewID)
    {
        PhotonView photonView = PhotonView.Find(viewID);
        if (photonView != null)
        {
            trackPlayer = photonView;
        }
        else
        {
            Debug.LogError("PhotonView with View ID " + viewID + " not found.");
        }
    }

    [PunRPC]
    public void SetTargetPlayer()
    {
        if(trackPlayer != null)
        {
            targetPlayer = trackPlayer.transform;
        }
    }
}
