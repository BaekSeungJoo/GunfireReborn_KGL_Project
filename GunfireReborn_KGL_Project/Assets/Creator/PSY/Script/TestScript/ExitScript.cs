using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Button exitButton;

    public UIManager uiManager;

    private void Start()
    {
        exitButton.transform.localScale = Vector3.zero;

        uiManager = GameObject.Find("@Managers").GetComponent<UIManager>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        exitButton.transform.localScale = Vector3.one;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        exitButton.transform.localScale = Vector3.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiManager.CheckActiveShopPopup())
        {
            uiManager?.SetActiveShopPopup(false);
        }   
    }
}
