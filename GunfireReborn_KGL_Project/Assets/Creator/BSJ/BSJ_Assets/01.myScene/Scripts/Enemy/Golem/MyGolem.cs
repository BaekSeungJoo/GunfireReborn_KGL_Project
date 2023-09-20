using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyGolem : MonoBehaviour
{
    public enum Type { Melee, Range, Boss }; // 타입 ( 근거리, 원거리, 보스)
    public Type enemyType;                  // 적 타입

    public float trackingSpeed;             // 추적 스피드
    public float trackingRange;             // 추적 거리
    public float attackRange;               // 공격 시작 범위

    // public Transform targetPlayer;          // 플레이어 위치
    // public PhotonView trackPlayer;           // 최종 추적할 플레이어
    public GameObject trackPlayer;
    public Animator animator;               // 애니메이터

    public bool isIdle;                     // 대기 상태
    public bool isTracking;                 // 추적 상태
    public bool isAttacking;                // 공격 상태

    public NavMeshAgent nav;                // 네비게이션 사용

    public AudioClip attackAudioClip01;     // 오디오 클립
    public AudioClip attackAudioClip02;     // 오디오 클립
    private AudioSource attackAudioSource;  // 오디오 소스

    public GameObject normalAttackEffect01;   // 공격 1 이펙트
    public GameObject normalAttackEffect02;   // 공격 1 이펙트

    private void Awake()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        attackAudioSource = GetComponent<AudioSource>();
    }

    // 공격 이펙트, 효과음
    public void Attack01_VFX()
    {
        normalAttackEffect01.SetActive(true);
        attackAudioSource.clip = attackAudioClip01;
        attackAudioSource.Play();
    }

    public void Attack02_VFX()
    {
        normalAttackEffect02.SetActive(true);
        attackAudioSource.clip = attackAudioClip02;
        attackAudioSource.Play();
    }

    public void AttackEnd_VFX()
    {
        normalAttackEffect01.SetActive(false);
        normalAttackEffect02.SetActive(false);
    }
}
