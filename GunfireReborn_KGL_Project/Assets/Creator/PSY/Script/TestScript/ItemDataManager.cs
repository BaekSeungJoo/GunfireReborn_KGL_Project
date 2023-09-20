using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아이템 데이터 정보 클래스
/// </summary>
public class ItemData
{
    public int index;    // 아이템 번호
    public string name;  // 아이템 이름
    public string info;  // 아이템 정보
    public int count;    // 아이템 갯수
    public int price;    // 아이템 가격
}

public class ItemDataManager : MonoBehaviour
{
    public List<ItemData> ItemList { get; private set; } = new List<ItemData>();  // 원본 아이템 데이터 

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

    public Sprite[] itemSprites;  // 아이템 이미지

    // TODO: 상수 ITEMCOUNTMAX 를 추가한다. 5로 초기화해준다.
    private const int ITEMCOUNTMAX = 5;

    private void Awake()
    {
        // TODO: ITEMCOUNTMAX 만큼 반복해서 itemList에 추가한다.
        for (int i = 0; i < ITEMCOUNTMAX; i++)
        {
            ItemData data = new ItemData();

            // TODO: 아이템의 번호, 이름, 정보, 갯수, 가격을 세팅해준다.
            data.index = i;
            data.name = itemNameList[i];
            data.info = ItemInfoList[i];
            data.count = 1;
            data.price = Random.Range(1, 100);

            ItemList.Add(data);

        }
    }
}
