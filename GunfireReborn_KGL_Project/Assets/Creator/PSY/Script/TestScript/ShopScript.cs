using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline bgOutline;

    private void Start()
    {
        bgOutline = GetComponent<Outline>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bgOutline.enabled = true;
        bgOutline.effectColor = new Color(255f, 222f, 73f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bgOutline.enabled = false;
    }
}
