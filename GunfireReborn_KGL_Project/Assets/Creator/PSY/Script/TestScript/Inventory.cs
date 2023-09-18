using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<InventoryBox> inventoryBoxeList = new List<InventoryBox>();

    private Transform invenBoxGroup;  
    public GameObject invenBoxPrefab;
    


    private void Start()
    {
        invenBoxGroup = GameObject.Find("ItemBoxGroup").GetComponent<Transform>();
    }
}
