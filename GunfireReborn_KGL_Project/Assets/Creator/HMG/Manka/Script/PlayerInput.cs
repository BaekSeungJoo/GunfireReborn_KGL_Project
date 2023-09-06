using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public string VMoveAxisName = "Vertical";
    public string HMoveAxisName = "Horizontal";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";
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
    public bool reload = default;
    public float RMove;
    public bool jump = default;
    public bool dash = default;
    public bool swap1 = default;
    public bool swap2 = default;
    public bool swap3 = default;
    public bool get = default;

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
        //reload에 관한 입력감지
        reload = Input.GetButtonDown(reloadButtonName);
        //Rmove에 관한 입력감지
        RMove = Input.GetAxis(RotateName);
        //jump에 관한 입력감지
        jump = Input.GetButtonDown(jumpName);
        //dash에 관한 입력감지
        dash = Input.GetButtonDown(dashName);
        //swap1에 관한 입력감지
        swap1 = Input.GetButtonDown(swap1Name);
        //swap2에 관한 입력감지
        swap2 = Input.GetButtonDown(swap2Name);
        //swap3에 관한 입력감지
        swap3 = Input.GetButtonDown(swap3Name);
        //get에 관한 입력감지
        get = Input.GetButton(getItem);

        //ToDo:
        //InputManager에서 fire,reload,dash,swap1,2,3, getItem처리를 해줘야한다.

        
    }
}
