using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SetDropVelocity : MonoBehaviourPun
{
    private Rigidbody rb;
    private float randonPosX;       // ������ �� Ƣ����� �ӵ��� �� x��
    private float randonPosY;       // ������ �� Ƣ����� �ӵ��� �� y��
    private float randonPosZ;       // ������ �� Ƣ����� �ӵ��� �� z��
    private Vector3 newVelocity;    // ������ �� Ƣ����� �ӵ�

    private Transform playerTransform;  // �÷��̾��� ��ġ
    private float moveSpeed = 5f;            // �÷��̾� ������ �ٰ����� �ӵ�
    
    private bool isMoveTowardPlayer = false;    // �÷��̾� �̵� ������ üũ
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        SetVelocity();
        rb.AddForce(newVelocity, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.transform.CompareTag("weapon"))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && photonView.IsMine)
        {
            playerTransform = other.transform;
            isMoveTowardPlayer = true;
        }
    }

    private void Update()
    {
        // �÷��̾� ������ �̵� ���� ���
        if(isMoveTowardPlayer && playerTransform != null)
        {
            // �÷��̾� ������ �̵�
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);
        }
    }



    public void SetVelocity()
    {
        randonPosX = Random.Range(0f, 0.1f);
        randonPosY = Random.Range(0f, 0.5f);
        randonPosZ = Random.Range(0f, 0.1f);
        newVelocity = new Vector3(randonPosX, randonPosY, randonPosZ);
    }
}