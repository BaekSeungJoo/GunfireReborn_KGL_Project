using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSmithManager : MonoBehaviour
{
    private BlackSmith[] blackSmiths;

    private int count = 3; // 잔여 강화 횟수

    #region 프로퍼티
    public int Count  // 잔여 강화 횟수 프로퍼티
    {
        get
        {
            return count;
        }
        set
        {
            count = value;

            for (int i = 0; i < blackSmiths.Length; i++)
            {
                // Debug.Log("들어옴?");

                blackSmiths[i].SetUpgradeCount(count);

                if (count <= 0)
                {
                    // Debug.Log("카운트 0인데 왜 해");
                    blackSmiths[i].SetUpgradeBT();
                }
            }
        }

    }
    #endregion


    private void Awake()
    {
        blackSmiths = FindObjectsOfType<BlackSmith>();  // BlackSmith 스크립트를 가지고 있는
                                                        // 오브젝트를 가져와서 배열에 넣는다.
    }

    /// <summary>
    /// 강화 버튼 Click 이벤트
    /// </summary>
    /// <param name="index">BlackSmith 배열의 인덱스</param>
    public void OnUpgrade(int index) 
    {
        Count--;

        blackSmiths[index].WeaponUpgradeCount++;    // 무기 강화 횟수 +1
        blackSmiths[index].SetUpgradeWeaponName();  

        blackSmiths[index].UpgradePrice += 100;     // 강화 코인 가격 +100
        blackSmiths[index].SetUpgradeCoin();
    }
}
