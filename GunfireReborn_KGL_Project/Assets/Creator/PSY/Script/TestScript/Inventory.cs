using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private ItemDataManager itemDataManager;

    private WeaponBox[] weaponBoxes;
    private InventoryBox[] inventoryBoxes;

    private void Start()
    {
        itemDataManager = GameObject.Find("@Managers").GetComponent<ItemDataManager>();

        #region F 스왑
        weaponBoxes = GetComponentsInChildren<WeaponBox>();  // 현재 스크립트를 가진 오브젝트의 모든 자식 오브젝트 중에서
                                                             // WeaponBox 클래스를 가진 자식들을 모두 가져오는 함수이다.

        /* 스왑할 무기 데이터 */
        weaponBoxes[0].SetData(new WeaponData("스나이퍼 +2", 20, 40, 60, "특수탄", "총의 정보입니다.", "사용 중"));
        weaponBoxes[1].SetData(new WeaponData("라이플 +4", 30, 60, 90, "특수탄2", "총의 정보입니다!", ""));
        #endregion

        inventoryBoxes = GetComponentsInChildren<InventoryBox>();

        //for ( int i = 0; i < itemDataManager.ItemList.Count; i++ )
        //{
        //    inventoryBoxes[i].SetItemData(itemDataManager.ItemList[i]);
        //}
    }

    private void Update()
    {
        #region F 스왑
        if ( Input.GetKeyDown(KeyCode.F) )
        {
            Swap( weaponBoxes[0],  weaponBoxes[1]);
        }
        #endregion
    }

    #region F 스왑
    /// <summary>
    /// 데이터 스왑 함수
    /// </summary>
    public void Swap( WeaponBox a ,  WeaponBox b)
    {
        WeaponData tempData = new WeaponData();

        tempData = a.data;
        
        a.SetData(b.data);
        b.SetData(tempData);
    }
    #endregion
}
