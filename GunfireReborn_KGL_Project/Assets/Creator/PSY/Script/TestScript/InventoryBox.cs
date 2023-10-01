using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBox : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private UIManager uiManager;

    private Inventory inventory;
    private ItemData itemData;
    private Image itemImage;
    private TextMeshProUGUI itemCountText;
    private Outline inventoryBoxOutLine;
    private Transform inventoryBoxSize;


    private int itemCount = 0;

    private playerBullet playerBullet;
    private playerHp playerHp;

    public ItemData ItemData { get { return itemData; } }  // ItemData 프로퍼티

    private void Awake()
    {
        uiManager = GameObject.Find("@Managers").GetComponent<UIManager>();

        inventory = GameObject.Find("InventoryCanvas").GetComponent<Inventory>();
        itemImage = transform.Find("ItemImage").GetComponent<Image>();
        itemCountText = transform.Find("ItemCount").GetComponent<TextMeshProUGUI>();
        inventoryBoxOutLine = GetComponent<Outline>();
        inventoryBoxSize = GetComponent<Transform>();
        playerBullet = GameObject.Find("Main Camera/unitychan_SSC001/WeaponPosition").GetComponent<playerBullet>();
        

        inventoryBoxOutLine.enabled = false;
        inventoryBoxSize.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        itemImage.color = new Color(0f, 0f, 0f, 0f);
        itemCountText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (itemData == null)
        {
            return;
        }

        // { "신기한 만두", "탄약 보급", "특수탄", "대형탄", "일반탄" };
        if (ItemData.index ==0)
        {
            playerHp = GameObject.Find("Cinemachine").transform.parent.gameObject.GetComponent<playerHp>();
            playerHp.curHealth += 40;
            if(playerHp.curHealth > 100)
            {
                playerHp.curHealth = 100;
            }
        }
        
        if (ItemData.index ==1)
        {

            playerBullet.remainNBullet += 60;
            playerBullet.remainBBullet += 30;

            if(playerBullet.remainNBullet > playerBullet.maxNBullet)
            { playerBullet.remainNBullet = playerBullet.maxNBullet; }
            if(playerBullet.remainBBullet > playerBullet.maxBBullet)
            { playerBullet.remainBBullet = playerBullet.maxBBullet; }
        }
        if (ItemData.index ==2)
        {

        }
        if (ItemData.index ==3)
        {
            playerBullet.remainBBullet += 30;

            if (playerBullet.remainBBullet > playerBullet.maxBBullet)
            { playerBullet.remainBBullet = playerBullet.maxBBullet; }
        }


        if ( ItemData.index == 4  )
        {
            playerBullet.remainNBullet += 60;

            if (playerBullet.remainNBullet > playerBullet.maxNBullet)
            { playerBullet.remainNBullet = playerBullet.maxNBullet; }

            // Debug.Log(playerBullet.remainNBullet);
        }

        SetItemCount(-1);  // 인벤토리에서 해당 아이템을 클릭하면 갯수가 감소한다.
        SetItemImage();

        if (itemCount <= 0)
        {
            itemData = null;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryBoxOutLine.enabled = true;
        inventoryBoxSize.localScale = new Vector3(1.2f , 1.2f , 1.0f);

        if (itemData != null)
        {
            uiManager.SetActiveToolTip(true);
            uiManager.SetToolTipText(itemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryBoxOutLine.enabled = false;
        inventoryBoxSize.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        uiManager.SetActiveToolTip(false);
    }

    #region 세팅
    /// <summary>
    /// 아이템 데이터 세팅 함수
    /// </summary>
    /// <param name="itemData">아이템 정보</param>
    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
    }

    /// <summary>
    /// 아이템 이미지 세팅 함수
    /// </summary>
    public void SetItemImage()
    {
        itemImage.sprite = inventory.ItemDataManager.itemSprites[itemData.index];
        itemImage.color = Color.white;

        if (itemCount <= 0)
        {
            itemImage.gameObject.SetActive(false);
        }
        else
        {
            itemImage.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 아이템 갯수 세팅 함수
    /// </summary>
    public void SetItemCount(int AddCount)
    {
        itemCount += AddCount;  // 추가한 아이템 갯수만큼 보유 개수를 증가시킨다.
        itemCountText.text = $"{itemCount}";  // UI에 표시

        // TODO: 0이하일 때, 텍스트가 지워져야함  음수면 0으로 변환시켜야함
        if (itemCount <= 0)
        {
            itemCountText.text = "";
            itemCount = 0;
        }
    }
    #endregion
}
