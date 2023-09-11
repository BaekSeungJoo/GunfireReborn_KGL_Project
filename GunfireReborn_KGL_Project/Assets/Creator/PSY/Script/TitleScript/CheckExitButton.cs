using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckExitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler , IPointerClickHandler
{
    public Image bt;

    private Color baseColor;
    private Color changeColor;

    public string checkType;

    public GameObject popupExit;

    private void Start()
    {
        bt = GetComponent<Image>();
        baseColor = bt.color;

        popupExit = GameObject.Find("PopupExit");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ColorUtility.TryParseHtmlString("#FFFFFFF", out changeColor);
        bt.color = changeColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bt.color = baseColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ( checkType == "Yes" )
        {
            Debug.Log("종료 완료");
            Application.Quit();
        }
        else
        {
            popupExit.SetActive(false);
        }
    }
}
