using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private ShopManager shopManager;
    private ItemDataManager itemDataManager;

    private WeaponBox[] weaponBoxes;
    private List<InventoryBox> inventoryBoxes;

    #region 프로퍼티
    public List<InventoryBox> InventoryBoxes { get { return inventoryBoxes; } }  // InventoryBoxes List 프로퍼티
    public ItemDataManager ItemDataManager { get { return itemDataManager; } }  // ItemDataManager 프로퍼티
    #endregion

    private void Start()
    {
        #region F 스왑
        weaponBoxes = GetComponentsInChildren<WeaponBox>();  // 현재 스크립트를 가진 오브젝트의 모든 자식 오브젝트 중에서
                                                             // WeaponBox 클래스를 가진 자식들을 모두 가져오는 함수이다.

        /* 스왑할 무기 데이터 */
        weaponBoxes[0].SetData(new WeaponData("스나이퍼 +2", 20, 40, 60, "특수탄", "총의 정보입니다.", "사용 중"));
        weaponBoxes[1].SetData(new WeaponData("라이플 +4", 30, 60, 90, "특수탄2", "총의 정보입니다!", ""));
        #endregion

        itemDataManager = GameObject.Find("@Managers").GetComponent<ItemDataManager>();
        shopManager = GameObject.Find("@Managers").GetComponent<ShopManager>();

        // ToList() : 배열을 리스트로 바꿔준다.
        inventoryBoxes = GameObject.Find("ItemInventoryFrame/ItemBoxGroup").GetComponentsInChildren<InventoryBox>().ToList();
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

    /// <summary>
    /// 구매 처리 함수
    /// </summary>
    /// <param name="buyItem">구매한 아이템</param>
    public void Trade( ItemData buyItem )
    {
        bool isSame = false;

        for ( int i = 0; i < inventoryBoxes.Count; i++ )
        {
            if (InventoryBoxes[i].ItemData == buyItem )
            {  // 구매한 아이템이 인벤토리에 이미 있는 아이템이라면
                inventoryBoxes[i].SetItemCount(buyItem.count);
                isSame = true;
                break;
            }
        }

        if ( !isSame )
        {
            for (int i = 0; i < inventoryBoxes.Count; i++)
            {
                if (inventoryBoxes[i].ItemData == null)
                {
                    inventoryBoxes[i].SetItemData(buyItem);         // 구매한 아이템을 비어있는 인벤토리에 넣는다.
                    inventoryBoxes[i].SetItemCount(buyItem.count);  // 구매한 아이템의 갯수만큼 인벤토리에 보유량을 늘린다.
                    inventoryBoxes[i].SetItemImage();               // 구매한 아이템의 이미지를 인벤토리에 넣는다.
                    break;
                }
            }
        }
        
    }
}
