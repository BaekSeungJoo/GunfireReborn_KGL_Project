using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public string VMoveAxisName = "Vertical";
    public string HMoveAxisName = "Horizontal";
    public string fireButtonName = "Fire1";
    public string RotateName = "Mouse X";
    public string jumpName = "Jump";
    public string dashName = "Dash";
    public string swap1Name = "Swap1";
    public string swap2Name = "Swap2";
    public string swap3Name = "Swap3";
    public string getItem = "Get";

    public float VMove;
    public float HMove;
    public bool fire = default;
    public float RMove;
    public bool jump = default;
    public bool dash = default;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //매 프레임 사용자 입력을 감지
        //ToDo:게임매니저의 인스턴스가 null이 아닌데 GameOver상태일때는 입력을 멈춰야함 if문 처리해야함
        //대쉬 상태에는 입력을 잠깐 못받게 하기 대쉬 false를 true로 바꾸는일은 Movement 스크립트에서 처리함
        if(dash == true)
        {
            return;
        }
        
        //Vmove에 관한 입력감지
        VMove = Input.GetAxis(VMoveAxisName);
        //Hmove에 관한 입력감지
        HMove = Input.GetAxis(HMoveAxisName);
        //fire에 관한 입력감지
        fire = Input.GetButton(fireButtonName);
        //Rmove에 관한 입력감지
        RMove = Input.GetAxis(RotateName);
        //jump에 관한 입력감지
        jump = Input.GetButtonDown(jumpName);
        //dash에 관한 입력감지
        dash = Input.GetButtonDown(dashName);

        //ToDo:
        //InputManager에서 fire,reload,dash,swap1,2,3, getItem처리를 해줘야한다.

        
    }
}
