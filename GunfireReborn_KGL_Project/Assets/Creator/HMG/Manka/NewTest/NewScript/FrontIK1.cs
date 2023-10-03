using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FrontIK1 : MonoBehaviour
{
    //웨폰 포지션
    public Transform FrontweaponPosition;
    //웨폰의 자식들을저장할배열
    public GameObject[] FrontWeaponChilds;
    //왼쪽 오른쪽손이 갈위치
    public Transform p_HandLeft;
    public Transform p_HandRight;
    public Animator IKAnimator;


    // Start is called before the first frame update
    void Start()
    {
            FrontWeaponChilds = new GameObject[3];
            IKAnimator = gameObject.GetComponent<Animator>();
            //웨폰들을  weaponchilds에 저장해야함
            FrontWeaponChilds = new GameObject[FrontweaponPosition.childCount];
            for (int i = 0; i < FrontweaponPosition.childCount; i++)
            {
                FrontWeaponChilds[i] = FrontweaponPosition.GetChild(i).gameObject;
            }
            ChangeIK("Pistol");
    }

    private void OnAnimatorIK(int layerIndex)
    {
            //왼손 조절하는코드
            IKAnimator.SetIKPosition(AvatarIKGoal.LeftHand, p_HandLeft.position);
            IKAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);

            //오른손 조절하는코드
            IKAnimator.SetIKPosition(AvatarIKGoal.RightHand, p_HandRight.position);
            IKAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
    }

    public void ChangeIK(string weaponName)
    {
        //for 문을 돌려서 웨폰중 이름이 weaponName과 같은것과
        //ex) weaponName이 Pistol일경우 Pistol 하위오브젝트에있는 leftGrap, rightGrap을 p_handLeft 와 p_handRight에 대입.
        for (int i = 0; i < FrontWeaponChilds.Length; i++)
        {
            if (FrontWeaponChilds[i].name == weaponName)
            {
                // IK를 맞추는 코드 추가
                p_HandLeft = FrontWeaponChilds[i].GetComponent<weapon>().leftGrap;
                p_HandRight = FrontWeaponChilds[i].GetComponent<weapon>().rightGrap;
            }
        }
    }

  
}
