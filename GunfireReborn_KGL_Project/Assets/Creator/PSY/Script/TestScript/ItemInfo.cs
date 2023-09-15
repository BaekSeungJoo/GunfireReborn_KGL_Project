using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    private TMP_Text itemName;
    private TMP_Text itemInfo;
    private Image itemImage;

    private void Start()
    {
        itemName = transform.GetChild(2).GetComponent<TMP_Text>();
        itemInfo = transform.GetChild(3).GetComponent<TMP_Text>();
        itemImage = transform.GetChild(5).GetChild(0).GetComponent<Image>();
    }

    public void SetItem(string itemname , string iteminfo , Image itemimage)
    {
        itemName.text = itemname;
        itemInfo.text = iteminfo;
        itemImage.sprite = itemimage.sprite;
    }

}
