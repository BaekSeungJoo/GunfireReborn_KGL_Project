using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    private MainUI mainUI;
    

    [SerializeField]
    private float walkSpeed;
    private Rigidbody playerRB;
    
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float dashSpeed;

    private Animator animator;
    //땅 착지여부
    private CapsuleCollider capsuleCollider;
    private bool isGround = true;
    private bool dashCool = false;

    float _moveDirX = default;
    float _moveDirZ = default;
    void Start()
    {
        mainUI = GameObject.Find("MainUICanvas").GetComponent<MainUI>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>(); 
        playerRB = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }
        MoveUni();
        TryJump();
        IsGround();
        Dash();
    }

    private void MoveUni()
    {
        _moveDirX = Input.GetAxisRaw("Horizontal");
        _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

        playerRB.MovePosition(transform.position + _velocity * Time.deltaTime);

        animator.SetFloat("H", _moveDirX);
        animator.SetFloat("V", _moveDirZ);
    }

    

    private  void TryJump()
    {    // Space 키를 누르고 플레이어가 지면에 있는 상태이며 "Groggy" 태그가 아닌 경우
        if (Input.GetKeyDown(KeyCode.Space)&& isGround == true && 
            !((gameObject.tag =="Groggy")==true))
        {
            // 플레이어의 Rigidbody에 위쪽 방향으로 점프 힘을 적용
            playerRB.velocity = transform.up * jumpForce;
            // 애니메이션에서 'Jump' 트리거를 활성화하여 점프 애니메이션을 재생
            animator.SetTrigger("Jump");
        }
    }
    private void IsGround()
    {
        // 플레이어의 아래 방향으로 레이캐스트를 발사하여 땅과 충돌 여부를 확인
        isGround = Physics.Raycast
        (transform.position,Vector3.down,capsuleCollider.bounds.extents.y+0.1f);
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)&& dashCool == false)
        {   //LeftShift키를 입력받고 쿨타임중이 아닐때
            // 대쉬 입력 방향을 캐릭터의 로컬 좌표계로 변환합니다.
            Vector3 dashDirection = transform.TransformDirection
            (new Vector3(_moveDirX, 0f, _moveDirZ).normalized);
            if (dashDirection.magnitude > 0.1f)
            {   // 대쉬 입력 방향이 존재하면 대쉬합니다.
                playerRB.velocity = dashDirection * dashSpeed;
                Invoke("StopDash", 0.1f);
            }
            else
            {
                // 대쉬 입력 방향이 없을 경우, 플레이어가 현재 바라보는 방향으로 대쉬합니다.
                dashDirection = transform.forward.normalized;
                playerRB.velocity = dashDirection * dashSpeed;
                Invoke("StopDash", 0.1f);
            }
            // 대쉬 쿨타임 이펙트
            mainUI.CountDashCoolTime();
            StartCoroutine(mainUI.DashEffect());
        }
    }
    private void StopDash()
    {
        dashCool = true;
        Invoke("DashCoolOn", 3f);
        playerRB.velocity = Vector3.zero;
    }
    private void DashCoolOn()
    {
        dashCool = false;
    }
}
