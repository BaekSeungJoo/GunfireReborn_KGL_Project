using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IK1 : MonoBehaviour

{
    //웨폰 포지션
    public Transform weaponPosition;
    //웨폰의 자식들을저장할배열
    private GameObject[] weaponChilds;
    //왼쪽 오른쪽손이 갈위치
    public Transform p_HandLeft;
    public Transform p_HandRight;
    //얼굴이 바라볼 오브젝트
    public Transform targetObj;
    private Animator IKAnimator;
    //ik를 작동할지 말지 정하는 변수
    public bool isIk;

    [Range(0, 1)]
    public float HandIKHandler = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        isIk = true;
        IKAnimator = GetComponent<Animator>();
        //Todo  웨폰의 자식들을  weaponchilds에 저장해야함
        weaponChilds = new GameObject[weaponPosition.childCount];
        for (int i = 0; i < weaponPosition.childCount; i++)
        {
            weaponChilds[i] = weaponPosition.GetChild(i).gameObject;
        }
        ChangeIK("Pistol");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isIk == true)
        {
            //얼굴이 위를바라보게 만드는코드
            IKAnimator.SetLookAtWeight(1);
            IKAnimator.SetLookAtPosition(targetObj.position);
            //왼손 조절하는코드
            IKAnimator.SetIKPosition(AvatarIKGoal.LeftHand, p_HandLeft.position);
            IKAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, HandIKHandler);
            //오른손 조절하는코드
            IKAnimator.SetIKPosition(AvatarIKGoal.RightHand, p_HandRight.position);
            IKAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandIKHandler);
        }
        else
        {
            // LookAt IK를 해제
            IKAnimator.SetLookAtWeight(0);
            // 왼손 IK를 해제
            IKAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            // 오른손 IK를 해제
            IKAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
        }
    }

    public void ChangeIK(string weaponName)
    {
        //Todo : for 문을 돌려서 weaponposition의 자식중  이름이 weaponName과 같은 녀석과 ik를 맞추는 코드를 작성해야한다.
        for (int i = 0; i < weaponChilds.Length; i++)
        {
            if (weaponChilds[i].name == weaponName)
            {
                // IK를 맞추는 코드 추가
                p_HandLeft = weaponChilds[i].GetComponent<weapon>().leftGrap;
                p_HandRight = weaponChilds[i].GetComponent<weapon>().rightGrap;
            }
        }
    }
}
