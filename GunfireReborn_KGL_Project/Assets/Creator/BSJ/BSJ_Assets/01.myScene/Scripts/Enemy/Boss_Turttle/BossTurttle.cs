using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossTurttle : MonoBehaviourPun
{
    public bool isIdle;
    public bool isPattern01;
    public bool isPattern02;
    public bool isPattern03;

    public int randomPatternNumber = -1;
    public int setPatternNumber;

    private Animator animator;
    private float patternThinkTime = 5f;
    private float patternThinkTimer = 0f;

    // 패턴 1
    public GameObject breath;                                               // 패턴 1 브레스 공격 패턴
    private int[] pattern01_first_lineAttackTrueOrFalse = new int[5];       // 해당 라인에서 공격을 할 것인지 아닌지 판단
    private int[] pattern01_second_lineAttackTrueOrFalse = new int[5];
    private int[] pattern01_third_lineAttackTrueOrFalse = new int[5];
    private int[] pattern01_fourth_lineAttackTrueOrFalse = new int[5];
    private int[] pattern01_fifth_lineAttackTrueOrFalse = new int[5];

    public GameObject[] pattern01_warning01;                                // 패턴 1 공격 전 경고 표시
    public GameObject[] pattern01_warning02;
    public GameObject[] pattern01_warning03;
    public GameObject[] pattern01_warning04;
    public GameObject[] pattern01_warning05;
    public GameObject[] pattern01_fire01;                                   // 패턴 1 공격
    public GameObject[] pattern01_fire02;
    public GameObject[] pattern01_fire03;
    public GameObject[] pattern01_fire04;
    public GameObject[] pattern01_fire05;

    // 패턴 2
    public List<PhotonView> playerWithTag = new List<PhotonView>();         // 씬에 있는 플레이어 찾기
    public GameObject[] pattern02_warnings;                                 // 패턴 2 공격 전 경고 표시
    public GameObject[] pattern02_bombs;                                    // 패턴 2 공격

    // 패턴 3
    public GameObject pattern03_sheild;                                     // 패턴 3 쉴드
    private int[] pattern03_first_AttackTrueOrFalse = new int[5];           // 해당 공격 위치 에서 공격을 할 것인지 아닌지 판단
    private int[] pattern03_second_AttackTrueOrFalse = new int[5];
    private int[] pattern03_third_AttackTrueOrFalse = new int[5];
    public GameObject[] pattern03_warning01;                                // 패턴 3 공격 전 경고 표시
    public GameObject[] pattern03_warning02;
    public GameObject[] pattern03_warning03;
    public GameObject[] pattern03_explosion01;                              // 패턴 3 공격
    public GameObject[] pattern03_explosion02;
    public GameObject[] pattern03_explosion03;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Idle", true);
        isIdle = true;
        isPattern01 = false;
        isPattern02 = false;
        isPattern03 = false;
    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            if (isIdle)
            {
                patternThinkTimer += Time.time;

                if (patternThinkTimer > patternThinkTime)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        DecidePattern();
                        photonView.RPC("Client_DecidePattern", RpcTarget.Others, randomPatternNumber);
                    }

                    SetPattern();
                    patternThinkTimer = 0;
                }
            }

            else if (!isIdle)
            {
                if (isPattern01)
                {
                    animator.SetBool("Attack01", true);
                    animator.SetBool("Idle", false);
                }
                if (isPattern02)
                {
                    animator.SetBool("Attack02", true);
                    animator.SetBool("Idle", false);
                }
                if (isPattern03)
                {
                    animator.SetBool("Attack03", true);
                    animator.SetBool("Idle", false);
                }
            }
        }
    }

    public void DecidePattern()
    {
        // 패턴 결정 (0, 1, 2)
        randomPatternNumber = Random.Range(0, 3);
        setPatternNumber = randomPatternNumber;
    }

    [PunRPC]
    public void Client_DecidePattern(int _randomPatternNumber)
    {
        randomPatternNumber = _randomPatternNumber;
        setPatternNumber = randomPatternNumber;
    }

    public void SetPattern()
    {
        if (setPatternNumber == -1)
        {
            isIdle = true;
            isPattern01 = false;
            isPattern02 = false;
            isPattern03 = false;
        }
        else if (setPatternNumber == 0) { isIdle = false; isPattern01 = true; }
        else if (setPatternNumber == 1) { isIdle = false; isPattern02 = true; }
        else if (setPatternNumber == 2) { isIdle = false; isPattern03 = true; }
    }


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
        // 마스터 클라이언트에 저장 용
        int[] master_Pattern01_first_lineAttackTrueOrFalse = new int[5];

        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for (int i = 0; i < pattern01_first_lineAttackTrueOrFalse.Length; i++)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                // 공격을 할지 안할지 정한다.
                master_Pattern01_first_lineAttackTrueOrFalse[i] = Random.Range(0, 2);

                // RPC로 랜덤으로 정한값 고정
                photonView.RPC("Pattern01_Deside_FirstAttack", RpcTarget.All, i, master_Pattern01_first_lineAttackTrueOrFalse[i]);
            }

            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if(pattern01_first_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_warning01[i].SetActive(true);
            }
        }
    }

    [PunRPC]
    public void Pattern01_Deside_FirstAttack(int index, int _master_Pattern01_first_lineAttackTrueOrFalse)
    {
        pattern01_first_lineAttackTrueOrFalse[index] = _master_Pattern01_first_lineAttackTrueOrFalse;
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
        // 마스터 클라이언트 저장 용
        int[] master_Pattern01_second_lineAttackTrueOrFalse = new int[5];

        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for (int i = 0; i < pattern01_second_lineAttackTrueOrFalse.Length; i++)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                // 공격을 할지 안할지 정한다.
                master_Pattern01_second_lineAttackTrueOrFalse[i] = Random.Range(0, 2);

                // RPC로 랜덤으로 정한값 고정
                photonView.RPC("Pattern01_Deside_SecondAttack", RpcTarget.All, i, master_Pattern01_second_lineAttackTrueOrFalse[i]);
            }

            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if (pattern01_second_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_warning02[i].SetActive(true);
            }
        }
    }

    [PunRPC]
    public void Pattern01_Deside_SecondAttack(int index, int _master_Pattern01_second_lineAttackTrueOrFalse)
    {
        pattern01_second_lineAttackTrueOrFalse[index] = _master_Pattern01_second_lineAttackTrueOrFalse;
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
        // 마스터 클라이언트 저장 용
        int[] master_Pattern01_third_lineAttackTrueOrFalse = new int[5];

        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for (int i = 0; i < pattern01_third_lineAttackTrueOrFalse.Length; i++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // 공격을 할지 안할지 정한다.
                master_Pattern01_third_lineAttackTrueOrFalse[i] = Random.Range(0, 2);

                // RPC로 랜덤으로 정한값 고정
                photonView.RPC("Pattern01_Deside_ThirdAttack", RpcTarget.All, i, master_Pattern01_third_lineAttackTrueOrFalse[i]);
            }  

            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if (pattern01_third_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_warning03[i].SetActive(true);
            }
        }
    }

    [PunRPC]
    public void Pattern01_Deside_ThirdAttack(int index, int _master_Pattern01_third_lineAttackTrueOrFalse)
    {
        pattern01_third_lineAttackTrueOrFalse[index] = _master_Pattern01_third_lineAttackTrueOrFalse;
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
        // 마스터 클라이언트 저장 용
        int[] master_Pattern01_fourth_lineAttackTrueOrFalse = new int[5];

        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for (int i = 0; i < pattern01_fourth_lineAttackTrueOrFalse.Length; i++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // 공격을 할지 안할지 정한다.
                master_Pattern01_fourth_lineAttackTrueOrFalse[i] = Random.Range(0, 2);

                // RPC로 랜덤으로 정한값 고정
                photonView.RPC("Pattern01_Deside_FourthAttack", RpcTarget.All, i, master_Pattern01_fourth_lineAttackTrueOrFalse[i]);
            }

            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if (pattern01_fourth_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_warning04[i].SetActive(true);
            }
        }
    }

    [PunRPC]
    public void Pattern01_Deside_FourthAttack(int index, int _master_Pattern01_fourth_lineAttackTrueOrFalse)
    {
        pattern01_fourth_lineAttackTrueOrFalse[index] = _master_Pattern01_fourth_lineAttackTrueOrFalse;
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
        // 마스터 클라이언트 저장 용
        int[] master_Pattern01_fifth_lineAttackTrueOrFalse = new int[5];

        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for (int i = 0; i < pattern01_fifth_lineAttackTrueOrFalse.Length; i++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // 공격을 할지 안할지 정한다.
                master_Pattern01_fifth_lineAttackTrueOrFalse[i] = Random.Range(0, 2);

                // RPC로 랜덤으로 정한값 고정
                photonView.RPC("Pattern01_Deside_FifthAttack", RpcTarget.All, i, master_Pattern01_fifth_lineAttackTrueOrFalse[i]);
            }

            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if (pattern01_fifth_lineAttackTrueOrFalse[i] == 1)
            {
                pattern01_warning05[i].SetActive(true);
            }
        }
    }

    [PunRPC]
    public void Pattern01_Deside_FifthAttack(int index, int _master_Pattern01_fifth_lineAttackTrueOrFalse)
    {
        pattern01_fifth_lineAttackTrueOrFalse[index] = _master_Pattern01_fifth_lineAttackTrueOrFalse;
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

    #region 패턴 2
    // 모든 플레이어의 위치를 구한다.
    public void Pattern02_FindAllPlayers()
    {
        PhotonView[] players = FindObjectsOfType<PhotonView>();
        playerWithTag = new List<PhotonView>();

        foreach (PhotonView player in players)
        {
            if (player.gameObject.CompareTag("Player"))
            {
                playerWithTag.Add(player);
            }
        }
    }

    // 그곳에 폭탄 경고를 활성화하고,
    public void Pattern02_Active_BoomWarningMark()
    {
        for(int i = 0; i < playerWithTag.Count; i++)
        {
            pattern02_warnings[i].transform.position = new Vector3 (playerWithTag[i].transform.position.x, -5, playerWithTag[i].transform.position.z);
            pattern02_warnings[i].SetActive(true);
        }
    }

    // 폭탄 경고를 비활성화한다.
    public void Pattern02_InActive_BoomWarningMark()
    {
        foreach(GameObject warning in pattern02_warnings)
        {
            warning.SetActive(false);
        }
    }

    // 잠시 뒤에 폭발시킨다.
    public void Pattern02_Do_BoomAttack()
    {
        for (int i = 0; i < playerWithTag.Count; i++)
        {
            pattern02_bombs[i].transform.position = pattern02_warnings[i].transform.position;
            pattern02_bombs[i].SetActive(true);
        }
    }

    // 공격 끝
    public void Pattern02_AttackEnd()
    {
        foreach (GameObject boom in pattern02_bombs)
        {
            boom.SetActive(false);
        }
    }
    #endregion

    #region 패턴 3
    // 공격을 막는 쉴드 활성화
    public void Pattern03_Active_Sheild()
    {
        pattern03_sheild.SetActive(true);
    }

    // 공격을 막는 쉴드 비활성화
    public void Pattern03_InActive_Sheild()
    {
        pattern03_sheild.SetActive(false);
    }

    // 패턴 3 공격01 표시
    public void Pattern03_Active_FirstExplosionWarningMark()
    {
        // 마스터 클라이언트에 저장 용
        int[] master_Pattern03_first_AttackTrueOrFalse = new int[5];

        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for (int i = 0; i < pattern03_first_AttackTrueOrFalse.Length; i++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // 공격을 할지 안할지 정한다.
                master_Pattern03_first_AttackTrueOrFalse[i] = Random.Range(0, 2);

                // RPC로 랜덤으로 정한값 고정
                photonView.RPC("Pattern03_Deside_FirstAttack", RpcTarget.All, i, master_Pattern03_first_AttackTrueOrFalse[i]);
            }
                
            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if (pattern03_first_AttackTrueOrFalse[i] == 1)
            {
                pattern03_warning01[i].SetActive(true);
            }
        }
    }

    [PunRPC]
    public void Pattern03_Deside_FirstAttack(int index, int _master_Pattern01_first_lineAttackTrueOrFalse)
    {
        pattern03_first_AttackTrueOrFalse[index] = _master_Pattern01_first_lineAttackTrueOrFalse;
    }


    // 패턴 3 공격01 표시 비활성화
    public void Pattern03_InActive_FirstExplosionWarningMark()
    {
        foreach (GameObject warning in pattern03_warning01)
        {
            warning.SetActive(false);
        }
    }

    // 패턴 3 공격 01 공격
    public void Pattern03_Do_FirstAttack()
    {
        for (int i = 0; i < pattern03_first_AttackTrueOrFalse.Length; i++)
        {
            if (pattern03_first_AttackTrueOrFalse[i] == 1)
            {
                pattern03_explosion01[i].SetActive(true);
            }
        }
    }

    // 패턴 3 공격 01 공격 끝
    public void Pattern03_End_FirstAttack()
    {
        foreach(GameObject explosion in pattern03_explosion01)
        {
            explosion.SetActive(false);
        }
    }


    // 패턴 3 공격02 표시
    public void Pattern03_Active_SecondExplosionWarningMark()
    {
        // 마스터 클라이언트에 저장 용
        int[] master_Pattern03_second_AttackTrueOrFalse = new int[5];

        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for (int i = 0; i < pattern03_second_AttackTrueOrFalse.Length; i++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // 공격을 할지 안할지 정한다.
                master_Pattern03_second_AttackTrueOrFalse[i] = Random.Range(0, 2);

                // RPC로 랜덤으로 정한값 고정
                photonView.RPC("Pattern03_Deside_SecondAttack", RpcTarget.All, i, master_Pattern03_second_AttackTrueOrFalse[i]);
            }
                
            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if (pattern03_second_AttackTrueOrFalse[i] == 1)
            {
                pattern03_warning02[i].SetActive(true);
            }
        }
    }

    [PunRPC]
    public void Pattern03_Deside_SecondAttack(int index, int _master_Pattern01_second_lineAttackTrueOrFalse)
    {
        pattern03_second_AttackTrueOrFalse[index] = _master_Pattern01_second_lineAttackTrueOrFalse;
    }


    // 패턴 3 공격02 표시 비활성화
    public void Pattern03_InActive_SecondExplosionWarningMark()
    {
        foreach (GameObject warning in pattern03_warning02)
        {
            warning.SetActive(false);
        }
    }

    // 패턴 3 공격 02 공격
    public void Pattern03_Do_SecondAttack()
    {
        for (int i = 0; i < pattern03_second_AttackTrueOrFalse.Length; i++)
        {
            if (pattern03_second_AttackTrueOrFalse[i] == 1)
            {
                pattern03_explosion02[i].SetActive(true);
            }
        }
    }

    // 패턴 3 공격 02 공격 끝
    public void Pattern03_End_SecondAttack()
    {
        foreach (GameObject explosion in pattern03_explosion02)
        {
            explosion.SetActive(false);
        }
    }

    // 패턴 3 공격03 표시
    public void Pattern03_Active_ThirdExplosionWarningMark()
    {
        // 마스터 클라이언트에 저장 용
        int[] master_Pattern03_third_AttackTrueOrFalse = new int[5];

        // 공격을 할 라인 [ 0 == 공격하지 않음 / 1 == 공격함 ]
        for (int i = 0; i < pattern03_third_AttackTrueOrFalse.Length; i++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // 공격을 할지 안할지 정한다.
                master_Pattern03_third_AttackTrueOrFalse[i] = Random.Range(0, 2);

                // RPC로 랜덤으로 정한값 고정
                photonView.RPC("Pattern03_Deside_ThirdAttack", RpcTarget.All, i, master_Pattern03_third_AttackTrueOrFalse[i]);
            }
                
            // 공격하는 것으로 정해졌다면 경고를 활성화한다.
            if (pattern03_third_AttackTrueOrFalse[i] == 1)
            {
                pattern03_warning03[i].SetActive(true);
            }
        }
    }

    [PunRPC]
    public void Pattern03_Deside_ThirdAttack(int index, int _master_Pattern01_third_lineAttackTrueOrFalse)
    {
        pattern03_third_AttackTrueOrFalse[index] = _master_Pattern01_third_lineAttackTrueOrFalse;
    }

    // 패턴 3 공격03 표시 비활성화
    public void Pattern03_InActive_ThirdExplosionWarningMark()
    {
        foreach (GameObject warning in pattern03_warning03)
        {
            warning.SetActive(false);
        }
    }

    // 패턴 3 공격 03 공격
    public void Pattern03_Do_ThirdAttack()
    {
        for (int i = 0; i < pattern03_third_AttackTrueOrFalse.Length; i++)
        {
            if (pattern03_third_AttackTrueOrFalse[i] == 1)
            {
                pattern03_explosion03[i].SetActive(true);
            }
        }
    }

    // 패턴 3 공격 03 공격 끝
    public void Pattern03_End_ThirdAttack()
    {
        foreach (GameObject explosion in pattern03_explosion03)
        {
            explosion.SetActive(false);
        }
    }
    #endregion

    // 패턴 종료
    public void Pattern_Exit()
    {
        setPatternNumber = -1;

        isIdle = true;
        isPattern01 = false;
        isPattern02 = false;
        isPattern03 = false;
        animator.SetBool("Idle", true);
        animator.SetBool("Attack01", false);
        animator.SetBool("Attack02", false);
        animator.SetBool("Attack03", false);
    }

}
