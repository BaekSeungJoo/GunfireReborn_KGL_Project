using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRigid = default;
    private Transform myEye = default;
    private Transform myBody = default;
    private Transform myHand = default;

    public Vector3 camRotate = default;

    public bool isShoot = false;
    public bool useSkill = false;

    public Vector3 hitPoint = Vector3.zero;
    private float speed = 500.0f;

    // Start is called before the first frame update
    void Start()
    {
        
        myRigid = GetComponent<Rigidbody>();
        myBody = transform.Find("Body").GetComponentInChildren<Transform>();
        myEye = transform.Find("CameraArm").GetComponentInChildren<Transform>();
        myHand = myBody.transform.Find("Hand").GetComponentInChildren<Transform>();
        //muzzle = myHand.transform.Find("")


        //Debug.Log(myBody.name);
        //Debug.Log(myEye.name);
        //Debug.Log(myHand.name);
        //Debug.Log(muzzle.name);
        //Debug.Log("총구 위치 : " + myEye.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //Shooting();
        CamControll();
        
    }

    private void Move()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 move = new Vector3 (h, 0f, v) * speed * Time.deltaTime;
        move.y = myRigid.velocity.y;        
        move = myBody.transform.TransformDirection(move);
        myRigid.velocity = move;       
    }

    private void Shooting()
    {
        // { 마우스 좌클릭
        if(Input.GetMouseButton(0))
        {
            isShoot = true;
            //Debug.DrawRay(myEye.transform.position, myEye.transform.forward * 20f, Color.yellow);

            //RaycastHit hit; 

            //if(Physics.Raycast(myEye.transform.position, myEye.transform.forward, out hit, 50f))
            //{               
            //    hitPoint = hit.point;
            //    //Debug.Log("레이 찍힌 지점 : " + hitPoint);
                
            //}            
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isShoot = false;
        }
        // } 마우스 좌클릭


        // { 마우스 우클릭
        if (Input.GetMouseButtonDown(1))
        {
            useSkill = true;
            Debug.DrawRay(myEye.transform.position, myEye.transform.forward * 20f, Color.yellow);

            RaycastHit hit;

            if (Physics.Raycast(myEye.transform.position, myEye.transform.forward, out hit, 50f))
            {
                hitPoint = hit.point;
                //Debug.Log("레이 찍힌 지점 : " + hitPoint);

            }

        }
        else
        {
            useSkill = false;
        }
        // } 마우스 우클릭
    }

    private void CamControll()
    {
        float v = -Input.GetAxisRaw("Mouse Y");
        float h = Input.GetAxisRaw("Mouse X");

        //Debug.Log("마우스 입력값 : " + v);
        camRotate = myEye.eulerAngles;
        camRotate = new Vector3(camRotate.x + v * 500f * Time.deltaTime, camRotate.y + h * 500f * Time.deltaTime, 0f);        

        //myEye.rotation = Quaternion.Slerp(myEye.transform.rotation, Quaternion.LookRotation(camRotate), 1f);
        //camRotate.x = Mathf.Clamp(camRotate.x, -90f, 90f);

        //if (camRotate.x >= 90f)
        //{
        //    camRotate.x = 90f;
        //}

        //if (camRotate.x <= -90f)
        //{
        //    camRotate.x = -90f;
        //}

        //Debug.Log(" 마우스 입력값 : " + v * 500f * Time.deltaTime);

        myEye.eulerAngles = camRotate;
        myHand.eulerAngles = camRotate;

        Vector3 look = camRotate;
        look.x = 0f;
        look.z = 0f;

        myBody.eulerAngles = look;
        //transform.forward = myBody.forward;
        //look.y = 0f;

        //transform.forward = look;

    }
}
