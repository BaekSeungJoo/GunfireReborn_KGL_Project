using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBox : MonoBehaviour
{
    // TODO : 상점에서 산 물품이 인벤토리 박스에 보여지고,
    // 클릭 이벤트를 만든다.

    // 인벤토리에 들어가는 정보들을 변수로 만들장
    // ex ) 아이템 개수 , 이름 , 이미지
    // 힌트 : 샵아이템박스 클래스참고

    private ShopItemBox ShopItemBox;

    private int itemCount;
    private string itemName;
    private Image itemImage;

    private void Start()
    {
    }
}
