using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBox : MonoBehaviour
{
    private ItemData itemData;
    private Image itemImage;
    private TextMeshProUGUI itemCountText;

    private void Start()
    {
        itemImage = transform.Find("ItemImage").GetComponent<Image>();
        itemCountText = transform.Find("ItemCount").GetComponent <TextMeshProUGUI>();

        itemImage.color = new Color(0f, 0f, 0f, 0f);
        itemCountText.text = "";
    }

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
    }
}
