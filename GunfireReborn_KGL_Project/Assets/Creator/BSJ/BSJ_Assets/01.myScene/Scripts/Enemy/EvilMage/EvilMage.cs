using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EvilMage : MeleeEnemyController
{
    public GameObject floorEffect;  // �ٴ� ����Ʈ
    public GameObject chargeEffect; // ���� ���� ����Ʈ
    public GameObject attackEffect; // ���� �� ����Ʈ
    public GameObject explosionEffect; // ��ź ���� �̺�Ʈ

    public Transform boomPos;       // ��ź ���� ��ġ
    public GameObject boomPrefab;   // ��ź ������ (������Ʈ Ǯ���� ������ ����� ����)
    public GameObject fakeBoom;     // ������ ���� ��¥ ��ź

    public Transform _TargetPlayer; // Ÿ�� �÷��̾�

    private void Awake()
    {
        enemyType = Type.Range;
        
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        isIdle = true;              // ��� ����
        isTracking = false;         // ���� ����
        isAttacking = false;        // ���� ����
    }

    private void Update()
    {
        // �⺻���´� ��� ����
        if (isTracking == false && isAttacking == false)
        {
            isIdle = true;
            animator.SetBool("Idle", true);
        }

        // �÷��̾���� ��ġ�� ���ؼ� ���� �Ÿ������� �ٰ����� ����
        FindClosestPlayer();
        _TargetPlayer = targetPlayer;

        // ���� ����� �ִٸ�
        if (targetPlayer != null)
        {
            // ���� �÷��̾ �ٶ󺻴�. (�극�� �� ���� �ٶ󺸰� �ϱ� ����)
            // transform.LookAt(targetPlayer);

            // y�� ȸ�� ����
            targetDirection = targetPlayer.position - transform.position;
            targetDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(targetDirection.normalized);

            if (isAttacking == false)
            {
                // ���� �÷��̾ ���ؼ� �̵��Ѵ�.
                nav.SetDestination(targetPlayer.position);
            }

            // ���� ( ���� �÷��̾ ���ݹ��� �ȿ� ������ )
            if (Vector3.Distance(transform.position, targetPlayer.position) <= attackRange)
            {
                StartAttack();
            }

            // ��� ( ���� ���� ������ �÷��̾ ������ )
            else if (Vector3.Distance(transform.position, targetPlayer.position) > trackingRange)
            {
                StartIdle();
                _TargetPlayer = null;
            }
        }
    }

    // ��ź ����
    public void InitBoom()
    {
        // ���� �ڵ�
        // GameObject boom = Instantiate(boomPrefab, boomPos.position, Quaternion.identity);
        // boom.transform.parent = transform;

        // �ڵ� ����
        // ��ź�� ���� ������ �ʱ�ȭ�Ѵ�.
        boomPrefab.transform.position = boomPos.transform.position;

        // �ڽĿ�����Ʈ�� ��ź�� Ȱ��ȭ ���ش�.
        boomPrefab.SetActive(true);
    }

    // ��¥ ��ź Ȱ��ȭ
    public void InitFakeBoom()
    {
        fakeBoom.gameObject.SetActive(true);
    }

    // ���� ������ ( �ִϸ��̼� �̺�Ʈ )
    public void EvilMage_Charging()
    {
        // �극�� ������ Ȱ��ȭ
        InitFakeBoom();
        floorEffect.gameObject.SetActive(true);
        chargeEffect.gameObject.SetActive(true);
    }


    // ���� ���� (�ִϸ��̼� �̺�Ʈ)
    public void EvilMage_Attack()
    {
        // Debug.Log("Attack");

        // ��ź ����
        InitBoom();

        // ���� ������ ����Ʈ ��Ȱ��ȭ
        chargeEffect.gameObject.SetActive(false);
        // �ٴ� ����Ʈ ��Ȱ��ȭ
        floorEffect.gameObject.SetActive(false);

        // ���� ����Ʈ Ȱ��ȭ
        attackEffect.gameObject.SetActive(true);
    }

    // ���� �� ( �ִϸ��̼� �̺�Ʈ )
    public void EvilMage_AttackEnd()
    {
        // ���� ��
        AttackEnd();

        // ���� ����Ʈ ��Ȱ��ȭ
        attackEffect.gameObject.SetActive(false);
    }

    // ���� ����Ʈ ����
    public void Active_BombEffect(Vector3 effectInitPos)
    {
        explosionEffect.transform.position = effectInitPos;
        explosionEffect.SetActive(true);
    }
}