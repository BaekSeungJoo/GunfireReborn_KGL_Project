using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private ItemData itemData;

    private ShopManager shopManager;
    public ShopItemBox mine;
    public GameObject soldOut;

    private RectTransform bgSize;
    private Outline bgOutline;

    private Vector3 baseSize;


    public TextMeshProUGUI logText;

    private ItemInfo itemInfo;

    private Image itemImage;

    public ItemData ItemData { get { return itemData; } }  // ItemData 프로퍼티

    private PlayerGold gold;

    private void Start()
    {

        mine = this;


        bgOutline = GetComponent<Outline>();

        bgSize = GetComponent<RectTransform>();
        baseSize = bgSize.localScale;

        logText.transform.parent.gameObject.SetActive(false);

        itemInfo = GameObject.Find("UI_Information").GetComponent<ItemInfo>();

        shopManager = GameObject.Find("@Managers").GetComponent<ShopManager>();

        itemImage = transform.GetChild(0).GetChild(2).GetComponent<Image>();

        itemData = shopManager.itemDataManager.ItemList[transform.GetSiblingIndex()];     
        
       
    }
    private void OnEnable()  // 해당 스크립트가 활성화 되는 순간에 동작하는 함수 
    {
        soldOut.SetActive(false);  // 매진 UI를 비활성화한다.
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bgSize.localScale = new Vector3(1.1f, 1.1f, 1f);
        bgOutline.enabled = true;
        bgOutline.effectColor = new Color(255f, 222f, 73f);


        itemInfo.gameObject.SetActive(true);
        itemInfo.SetItem(shopManager.itemDataManager.ItemNameList[transform.GetSiblingIndex()],
            shopManager.itemDataManager.ItemInfoList[transform.GetSiblingIndex()],
            itemImage);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bgOutline.enabled = false;
        bgSize.localScale = baseSize;

        itemInfo.gameObject.SetActive(false);

    }

    // 클릭 시 아이템 구매 로직 
    public void OnPointerClick(PointerEventData eventData)
    {
        // { "신기한 만두", "탄약 보급", "특수탄", "대형탄", "일반탄" };
        gold = GameObject.Find("Cinemachine").transform.parent.gameObject.GetComponent<PlayerGold>();
        // Coin 쓸 곳 
        if ( ItemData.index == 0 )
        {
            // 코인이 부족하면 if ( ) { return; }
            if (gold.Gold <70)
            {
                return;
            }
            // 해당 인덱스 물품의 가격만큼 코인
            gold.Gold -= 70;
        }
        if (ItemData.index == 1)
        {
            // 코인이 부족하면 if ( ) { return; }
            if (gold.Gold < 70)
            {
                return;
            }
            // 해당 인덱스 물품의 가격만큼 코인
            gold.Gold -= 70;

            Debug.Log("삼");
        }
        if (ItemData.index == 2)
        {
            // 코인이 부족하면 if ( ) { return; }
            if (gold.Gold < 70)
            {
                return;
            }
            // 해당 인덱스 물품의 가격만큼 코인
            gold.Gold -= 70;

            Debug.Log("삼");
        }
        if (ItemData.index == 3)
        {
            // 코인이 부족하면 if ( ) { return; }
            if (gold.Gold < 70)
            {
                return;
            }
            // 해당 인덱스 물품의 가격만큼 코인
            gold.Gold -= 70;

            Debug.Log("삼");
        }
        logText.transform.parent.gameObject.SetActive(true);

        string styledItemName = $"<color=#FFFB00><size=28>" +
            $"{shopManager.itemDataManager.ItemNameList[transform.GetSiblingIndex()]}</size></color>";

        logText.text = $"{styledItemName} 구매했습니다";

        bgOutline.enabled = false;
        bgSize.localScale = baseSize;
        itemInfo.gameObject.SetActive(false);

        soldOut.SetActive(true);

        mine.enabled = false;

        // 구매한 아이템을 인벤토리에 추가한다.
        shopManager.Inventory.Trade(itemData);

        Invoke("SetActiveLogText", 1.5f);
    }

    private void SetActiveLogText()
    {
        logText.transform.parent.gameObject.SetActive(false);
    }
}
