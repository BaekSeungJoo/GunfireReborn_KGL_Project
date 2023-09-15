using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private UIManager uiManager;

    #region ItemList
    private List<string> itemNameList = new List<string> { "신기한 만두", "탄약 보급", "특수탄", "대형탄", "일반탄" };
    private List<string> itemInfoList = new List<string>
    {"신기한 만두. 속에 뭐가 들었는지는 몰라도, 먹으면 30%의 생명력을 회복",
     "모든 탄약을 보충하여 꽉 채운다",
     "특수 탄약을 보충하여 꽉 채운다",
     "대형 탄약을 보충하여 꽉 채운다",
     "일반 탄약을 보충하여 꽉 채운다"};

    /// <summary>
    /// ItemNameList 프로퍼티
    /// </summary>
    public List<string> ItemNameList { get { return itemNameList; } }
    /// <summary>
    /// ItemInfoList 프로퍼티
    /// </summary>
    public List<string> ItemInfoList { get { return itemInfoList; } }
    #endregion

    #region Reroll
    private int count = 3;
    /// <summary>
    /// Reroll Count 프로퍼티
    /// </summary>
    public int Count
    {
        get
        {
            return count;
        }
        set
        {
            count = value;

            if (count >= 0)
            {
                PlayerTest player = GameObject.Find("Player").GetComponent<PlayerTest>();
                for (int i = 0; i < player.shopScripts.Count; i++)
                {
                    player.shopScripts[i].soldOut.SetActive(false);
                    player.shopScripts[i].enabled = true;
                }
            }
        }
    }
    #endregion

    private void Start()
    {
        uiManager = GameObject.Find("@Managers").GetComponent<UIManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && uiManager.CheckActiveShopPopup())
        {
            uiManager.SetActiveShopPopup(false);

        }
    }
}
