using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private ShopManager shopManager;
    public ShopItemBox mine;
    public GameObject soldOut;

    private RectTransform bgSize;
    private Outline bgOutline;

    private Vector3 baseSize;


    public TextMeshProUGUI logText;

    private ItemInfo itemInfo;

    private Image itemImage;

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
        itemInfo.SetItem(shopManager.ItemNameList[transform.GetSiblingIndex()],
            shopManager.ItemInfoList[transform.GetSiblingIndex()],
            itemImage);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bgOutline.enabled = false;
        bgSize.localScale = baseSize;

        itemInfo.gameObject.SetActive(false);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        logText.transform.parent.gameObject.SetActive(true);

        string styledItemName = $"<color=#FFFB00><size=28>{shopManager.ItemNameList[transform.GetSiblingIndex()]}</size></color>";

        logText.text = $"{styledItemName} 구매했습니다";

        bgOutline.enabled = false;
        bgSize.localScale = baseSize;
        itemInfo.gameObject.SetActive(false);

        Debug.Log("1"); 
        soldOut.SetActive(true);

        mine.enabled = false;

        Invoke("SetActiveLogText", 1.5f);
    }

    private void SetActiveLogText()
    {
        logText.transform.parent.gameObject.SetActive(false);
    }
}
