using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ShopItemBox mine;
    public GameObject info;
    public GameObject soldOut;

    private RectTransform bgSize;
    private Outline bgOutline;

    private Vector3 baseSize;


    private void Start()
    {
        mine = this;

        bgOutline = GetComponent<Outline>();

        bgSize = GetComponent<RectTransform>();
        baseSize = bgSize.localScale;
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

        info.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bgOutline.enabled = false;
        bgSize.localScale = baseSize;

        info.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bgOutline.enabled = false;
        bgSize.localScale = baseSize;
        info.SetActive(false);

        soldOut.SetActive(true);

        mine.enabled = false;  
    }
}
