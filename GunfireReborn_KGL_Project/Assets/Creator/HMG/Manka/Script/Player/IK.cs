using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IK : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        IKAnimator = GetComponent<Animator>();
        //Todo  웨폰의 자식들을  weaponchilds에 저장해야함
        weaponChilds = new GameObject[weaponPosition.childCount];
        for (int i = 0; i < weaponPosition.childCount; i++)
        {
            weaponChilds[i] = weaponPosition.GetChild(i).gameObject;
        }

        //처음에 활성화할 IK와 무기
        weaponChilds[1].SetActive(true);
        ChangeIK("Rifle");
        //라이플을 활성화함
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnAnimatorIK(int layerIndex)
    {   
        //얼굴이 위를바라보게 만드는코드
        IKAnimator.SetLookAtWeight(1);
        IKAnimator.SetLookAtPosition(targetObj.position);
        
        //왼손 조절하는코드
        IKAnimator.SetIKPosition(AvatarIKGoal.LeftHand, p_HandLeft.position);
        IKAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        IKAnimator.SetIKRotation(AvatarIKGoal.LeftHand, p_HandLeft.rotation);
        IKAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        
        //오른손 조절하는코드
        IKAnimator.SetIKPosition(AvatarIKGoal.RightHand, p_HandRight.position);
        IKAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        IKAnimator.SetIKRotation(AvatarIKGoal.RightHand, p_HandRight.rotation);
        IKAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

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
