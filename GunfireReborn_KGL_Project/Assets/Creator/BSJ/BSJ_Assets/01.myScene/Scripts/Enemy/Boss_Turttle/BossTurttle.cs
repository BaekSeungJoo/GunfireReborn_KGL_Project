using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurttle : MonoBehaviour
{
    // 패턴 1
    public GameObject breath;
    int[] pattern01_first_lineAttackTrueOrFalse = new int[5];
    int[] pattern01_second_lineAttackTrueOrFalse = new int[5];
    int[] pattern01_third_lineAttackTrueOrFalse = new int[5];
    int[] pattern01_fourth_lineAttackTrueOrFalse = new int[5];
    int[] pattern01_fifth_lineAttackTrueOrFalse = new int[5];
    public GameObject[] pattern01_warning01;
    public GameObject[] pattern01_warning02;
    public GameObject[] pattern01_warning03;
    public GameObject[] pattern01_warning04;
    public GameObject[] pattern01_warning05;
    public GameObject[] pattern01_fire01;
    public GameObject[] pattern01_fire02;
    public GameObject[] pattern01_fire03;
    public GameObject[] pattern01_fire04;
    public GameObject[] pattern01_fire05;


    // 체력
    // 패턴 1 불꽃 데미지
    // 패턴 2 바닥찍기(일시 무적)
    // 패턴 3 플레이어 공격 무시 (일시 무적) || 데미지를 준 플레이어에게 데미지만 반사 (총알 반사는 이펙트로 처리)

    #region 패턴 1

    // 패턴 1 _ 브레스 활성화
    public void Pattern01_ActiveBreath()
    {
        breath.SetActive(true);
    }

    // 패턴 1 _ 브레스 비활성화
    public void Pattern01_InActiveBreath()
    {
        breath.SetActive(false);
    }

    // 패턴 1 _ 1번 공격 범위 경고 표시 ( 애니메이션 이펙트 )
    public void Pattern01_ActiveFirst_AttackWarningMark()
    {
        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for(int i = 0; i < pattern01_first_lineAttackTrueOrFalse.Length; i++)
        {
            // 공격을 할지 안할지 정한다.
            pattern01_first_lineAttackTrueOrFalse[i] = Random.Range(0, 2);

            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if(pattern01_first_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_warning01[i].SetActive(true);
            }
        }
    }

    // 패턴 1 _ 1번 공격 범위 경고 표시 종료 ( 애니메이션 이펙트 )
    public void Pattern01_InActiveFirst_AttackWarningMark()
    {
        foreach(GameObject warning in pattern01_warning01)
        {
            warning.SetActive(false);
        }
    }

    // 패턴 1 _ 1번 공격 실행
    public void Pattern01_Do_FirstAttack()
    {
        for(int i = 0; i < pattern01_first_lineAttackTrueOrFalse.Length; i++)
        {
            if (pattern01_first_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_fire01[i].SetActive(true);
            }
        }
    }

    // 패턴 1 _ 2번 공격 범위 경고 표시 ( 애니메이션 이펙트 )
    public void Pattern01_ActiveSecond_AttackWarningMark()
    {
        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for (int i = 0; i < pattern01_second_lineAttackTrueOrFalse.Length; i++)
        {
            // 공격을 할지 안할지 정한다.
            pattern01_second_lineAttackTrueOrFalse[i] = Random.Range(0, 2);

            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if (pattern01_second_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_warning02[i].SetActive(true);
            }
        }
    }

    // 패턴 1 _ 2번 공격 범위 경고 표시 종료 ( 애니메이션 이펙트 )
    public void Pattern01_InActiveSecond_AttackWarningMark()
    {
        foreach (GameObject warning in pattern01_warning02)
        {
            warning.SetActive(false);
        }
    }

    // 패턴 1 _ 2번 공격 실행
    public void Pattern01_Do_SecondAttack()
    {
        for (int i = 0; i < pattern01_second_lineAttackTrueOrFalse.Length; i++)
        {
            if (pattern01_second_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_fire02[i].SetActive(true);
            }
        }
    }

    // 패턴 1 _ 3번 공격 범위 경고 표시 ( 애니메이션 이펙트 )
    public void Pattern01_ActiveThird_AttackWarningMark()
    {
        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for (int i = 0; i < pattern01_third_lineAttackTrueOrFalse.Length; i++)
        {
            // 공격을 할지 안할지 정한다.
            pattern01_third_lineAttackTrueOrFalse[i] = Random.Range(0, 2);

            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if (pattern01_third_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_warning03[i].SetActive(true);
            }
        }
    }

    // 패턴 1 _ 3번 공격 범위 경고 표시 종료 ( 애니메이션 이펙트 )
    public void Pattern01_InActiveThird_AttackWarningMark()
    {
        foreach (GameObject warning in pattern01_warning03)
        {
            warning.SetActive(false);
        }
    }

    // 패턴 1 _ 3번 공격 실행
    public void Pattern01_Do_ThirdAttack()
    {
        for (int i = 0; i < pattern01_third_lineAttackTrueOrFalse.Length; i++)
        {
            if (pattern01_third_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_fire03[i].SetActive(true);
            }
        }
    }

    // 패턴 1 _ 4번 공격 범위 경고 표시 ( 애니메이션 이펙트 )
    public void Pattern01_ActiveFourth_AttackWarningMark()
    {
        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for (int i = 0; i < pattern01_fourth_lineAttackTrueOrFalse.Length; i++)
        {
            // 공격을 할지 안할지 정한다.
            pattern01_fourth_lineAttackTrueOrFalse[i] = Random.Range(0, 2);

            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if (pattern01_fourth_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_warning04[i].SetActive(true);
            }
        }
    }

    // 패턴 1 _ 4번 공격 범위 경고 표시 종료 ( 애니메이션 이펙트 )
    public void Pattern01_InActiveFourth_AttackWarningMark()
    {
        foreach (GameObject warning in pattern01_warning04)
        {
            warning.SetActive(false);
        }
    }

    // 패턴 1 _ 4번 공격 실행
    public void Pattern01_Do_FourthAttack()
    {
        for (int i = 0; i < pattern01_fourth_lineAttackTrueOrFalse.Length; i++)
        {
            if (pattern01_fourth_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_fire04[i].SetActive(true);
            }
        }
    }

    // 패턴 1 _ 5번 공격 범위 경고 표시 ( 애니메이션 이펙트 )
    public void Pattern01_ActiveFifth_AttackWarningMark()
    {
        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for (int i = 0; i < pattern01_fifth_lineAttackTrueOrFalse.Length; i++)
        {
            // 공격을 할지 안할지 정한다.
            pattern01_fifth_lineAttackTrueOrFalse[i] = Random.Range(0, 2);

            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if (pattern01_fifth_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_warning05[i].SetActive(true);
            }
        }
    }

    // 패턴 1 _ 5번 공격 범위 경고 표시 종료 ( 애니메이션 이펙트 )
    public void Pattern01_InActiveFifth_AttackWarningMark()
    {
        foreach (GameObject warning in pattern01_warning05)
        {
            warning.SetActive(false);
        }
    }

    // 패턴 1 _ 5번 공격 실행
    public void Pattern01_Do_FifthAttack()
    {
        for (int i = 0; i < pattern01_fifth_lineAttackTrueOrFalse.Length; i++)
        {
            if (pattern01_fifth_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_fire05[i].SetActive(true);
            }
        }
    }

    #endregion 패턴 1 끝
}
