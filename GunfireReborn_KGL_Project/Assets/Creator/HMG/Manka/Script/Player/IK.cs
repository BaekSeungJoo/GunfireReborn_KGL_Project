using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour
{
    //웨폰 포지션
    public Transform weaponPosition;

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
    }
}
