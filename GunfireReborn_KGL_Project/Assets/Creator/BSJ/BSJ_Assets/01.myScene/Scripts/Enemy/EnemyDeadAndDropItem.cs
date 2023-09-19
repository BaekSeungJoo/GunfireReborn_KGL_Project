using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadAndDropItem : MonoBehaviour
{
    private SetDropItem _SetDropItem;
    private GameObject _ItemDropManager;

    private void Awake()
    {
        // 하이어라키 창에서 _ItemDropManager 오브젝트를 찾아온다.
        _ItemDropManager = GameObject.Find("ItemDropManager");

        if( _ItemDropManager != null )
        {
            _SetDropItem = _ItemDropManager.GetComponent<SetDropItem>();
        }
    }

    private void OnDisable()
    {
        _SetDropItem.DropItem(gameObject.transform);
    }
}
