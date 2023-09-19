using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 7f;
    public float jumpForce = 5f;
    public float dashSpeed;
    public bool isJumping;
    public bool dashCool;
    
    public Vector3 movement;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private float lookSensitivity =5f;

    // TEST : 
    float y_Rotation = default;
    Quaternion rotationY = default;
    // Start is called before the first frame update
    void Start()
    {
        //if (!photonView.IsMine)
        //{
        //    return;
        //}
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
        //Cursor.visible = false; // 마우스 숨기기

        //playerRigidbody.transform.position = new Vector3 (transform.position.x, 5, transform.position.z);

        Dash();

        Move();

        Rotate();
    }

    private void Move()
    {
        movement.x = playerInput.HMove;
        movement.z = playerInput.VMove;
        if (playerInput.jump == true && isJumping == false)
        { //플레이어가 점프중이아니면서 점프키를 눌렀을때 점프하도록 만들음
            playerAnimator.Play("Jumping", -1, 0);
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpForce, playerRigidbody.velocity.z);
            playerInput.jump = false;
            isJumping = true;
        }
        else if(!(isJumping == true))
        { //플레이어가 점프중이 아니라면 이동이 가능하게 만들음
            playerRigidbody.velocity = transform.TransformDirection(movement * moveSpeed)+ Vector3.up * playerRigidbody.velocity.y;
            
           /* Vector3 movementX = transform.right * playerInput.HMove;
            Vector3 movementZ = transform.right * playerInput.VMove;

            movement = (movementX + movementZ).normalized*moveSpeed;

            playerRigidbody.MovePosition(transform.position + movement* Time.deltaTime);*/
            
            playerAnimator.SetFloat("H", movement.x); 
            playerAnimator.SetFloat("V", movement.z);
        }
    }
    private void Rotate()
    {
        

       // Debug.LogFormat("before{0}", gameObject.transform.position.y);
        float y_Rotation = Input.GetAxis("Mouse X");

        //Debug.LogFormat("{0}", y_Rotation);
        Vector3 rotationY = new Vector3(0f, y_Rotation*lookSensitivity, 0f); //* lookSensitivity;
        //Debug.LogFormat("{0}", rotationY);
        /* // TEST : 
         y_Rotation = Input.GetAxisRaw("Mouse X");
         rotationY = Quaternion.Euler(0f, y_Rotation, 0f) ;
        playerRigidbody.MoveRotation(playerRigidbody.rotation * rotationY);

         Debug.LogFormat("{0}", playerRigidbody.velocity);*/
        playerRigidbody.rotation *= Quaternion.Euler(rotationY);
        Debug.LogFormat("{0}", playerRigidbody.position.y);

        /*Debug.LogFormat("after{0}", gameObject.transform.position.y);*/
        //playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0f, playerInput.RMove * rotateSpeed, 0f);

        //transform.Rotate(0f, y_Rotation * rotateSpeed, 0f);
        //Debug.LogFormat("{0}", playerRigidbody.velocity);

    }

    private void Dash()
    {
        if (playerInput.dash == true && dashCool ==false)
        {
         // 대쉬 입력 방향을 캐릭터의 로컬 좌표계로 변환합니다.
            Vector3 dashDirection = transform.TransformDirection(new Vector3(playerInput.HMove, 0f, playerInput.VMove).normalized);

            // 대쉬 입력 방향이 존재하면 대쉬합니다.
            if (dashDirection.magnitude > 0.1f)
            {
                playerRigidbody.velocity = dashDirection * dashSpeed;
                Invoke("StopDash", 0.1f);
                Debug.LogFormat("대쉬했음");
            }
            else
            {
                // 대쉬 입력 방향이 없을 경우, 플레이어가 현재 바라보는 방향으로 대쉬합니다.
                dashDirection = transform.forward.normalized;
                playerRigidbody.velocity = dashDirection * dashSpeed;
                Invoke("StopDash", 0.1f);
                Debug.LogFormat("대쉬했음");
            }
        }
    }

    private void StopDash()
    {
        playerInput.dash = false;
        dashCool = true;
        Invoke("DashCoolOn", 3f);
        Debug.LogFormat("쿨타임중");
    }
    private void DashCoolOn()
    {
        dashCool = false;
        playerInput.dash = false;
        //선입력문제를 해결하려고 false를 쿨타임 다찰떄 false로 만들어놓음
        Debug.LogFormat("쿨타임끝");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            isJumping = false;
        }
    }
}
