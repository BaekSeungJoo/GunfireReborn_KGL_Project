using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private UIManager uiManager;

    public List<ShopItemBox> shopScripts = new List<ShopItemBox>();  // 여러 곳에 있는 ShopScript를 List에 담는다

    private float speed = 5f;

    private bool isShop = false;
    private bool isBlackSmith = false;

    private void Awake()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");  // Item 태그가 달린 오브젝트를 배열에 넣는다.
                                                                         // 해당 오브젝트들이 ShopScript를 가지고 있기 때문.
        for (int i = 0; i < items.Length; i++)
        {
            shopScripts.Add(items[i].GetComponent<ShopItemBox>());  // 가지고 온 태그가 아이템인 오브젝트들에서 ShopScript를 뽑아서 List에 넣는다.
        }
    }

    private void Start()
    {
        uiManager = GameObject.Find("@Managers").GetComponent<UIManager>();
    }
    private void Update()
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        //transform.position += Time.deltaTime * speed * new Vector3(h, 0f, v);

        if (isShop && Input.GetKeyDown(KeyCode.F))
        {
            uiManager.SetActiveShopPopup(true);

            for ( int i = 0; i < shopScripts.Count; i++ )
            {
                shopScripts[i].enabled = true;  // 상점이 켜지는 순간에 ShopScript를 활성화한다.
            }
        }

        if ( isBlackSmith && Input.GetKeyDown(KeyCode.F))
        {
            uiManager.SetActiveBlackSmith(true);
        }

        if ( Input.GetKeyDown(KeyCode.Tab) )
        {
            uiManager.SetActiveInven(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Shop"))
        {
            isShop = true;
        }
        else if ( other.tag == ("BlackSmith"))
        {
            isBlackSmith = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isShop = false;
        isBlackSmith = false;
    }
}
