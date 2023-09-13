using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FrontIK : MonoBehaviour
{
    //���� ������
    public Transform FrontweaponPosition;
    //������ �ڽĵ��������ҹ迭
    private GameObject[] FrontWeaponChilds;
    //���� �����ʼ��� ����ġ
    public Transform p_HandLeft;
    public Transform p_HandRight;
    private Animator IKAnimator;
    // Start is called before the first frame update
    void Start()
    {
        IKAnimator = gameObject.GetComponent<Animator>();
        //Todo  ������ �ڽĵ���  weaponchilds�� �����ؾ���
        FrontWeaponChilds = new GameObject[FrontweaponPosition.childCount];
        for (int i = 0; i < FrontweaponPosition.childCount; i++)
        {
            FrontWeaponChilds[i] = FrontweaponPosition.GetChild(i).gameObject;
        }
        ChangeIK("Pistol");
        //ChangeIK("Rifle");
        //ChangeIK("Shotgun");

        for (int i =0; i< FrontweaponPosition.childCount; i++)
        {
            Debug.LogFormat("{0}", FrontWeaponChilds[i].name);
        }
        //ó������ �⺻������ Ȱ��ȭ��\
        //ChangeIK("Shotgun");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnAnimatorIK(int layerIndex)
    {   
        //�޼� �����ϴ��ڵ�
        IKAnimator.SetIKPosition(AvatarIKGoal.LeftHand, p_HandLeft.position);
        IKAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        //IKAnimator.SetIKRotation(AvatarIKGoal.LeftHand, p_HandLeft.rotation);
        //IKAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        
        //������ �����ϴ��ڵ�
        IKAnimator.SetIKPosition(AvatarIKGoal.RightHand, p_HandRight.position);
        IKAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        //IKAnimator.SetIKRotation(AvatarIKGoal.RightHand, p_HandRight.rotation);
        //IKAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

    }

    public void ChangeIK(string weaponName)
    {
        Debug.Log("Front ü������");
        //Todo : for ���� ������ weaponposition�� �ڽ���  �̸��� weaponName�� ���� �༮�� ik�� ���ߴ� �ڵ带 �ۼ��ؾ��Ѵ�.
        for (int i = 0; i < FrontWeaponChilds.Length; i++)
        {
            if (FrontWeaponChilds[i].name == weaponName)
            {
                Debug.Log("Front ����ȵ����°ž�?");
                // IK�� ���ߴ� �ڵ� �߰�
                p_HandLeft = FrontWeaponChilds[i].GetComponent<weapon>().leftGrap;
                p_HandRight = FrontWeaponChilds[i].GetComponent<weapon>().rightGrap;
            }
        }
    }
}