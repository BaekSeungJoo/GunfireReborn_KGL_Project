using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SetDropItem : MonoBehaviour
{
    public List<GameObject> dropItems;

    private int coinCount = 1;
    private bool dropBullet01 = false;
    private bool dropBullet02 = false;
    private bool dropGun01 = false;
    private bool dropGun02 = false;

    public void DropItem(Transform deadEnemyPos)
    {
        // 드랍 아이템 정하기
        ChooseDropItems();

        // 코인 생성
        for (int i = 0; i < coinCount; i++)
        {
            GameObject coin = Instantiate(dropItems[0], gameObject.transform);
            coin.transform.position = deadEnemyPos.position;
            coin.SetActive(true);
        }

        // 총알 1 드랍
        if(dropBullet01 == true)
        {
            GameObject bullet01 = Instantiate(dropItems[1], gameObject.transform);
            bullet01.transform.position = deadEnemyPos.position;
            bullet01.SetActive(true);
        }

        // 총알 2 드랍
        if (dropBullet02 == true)
        {
            GameObject bullet02 = Instantiate(dropItems[2], gameObject.transform);
            bullet02.transform.position = deadEnemyPos.position;
            bullet02.SetActive(true);
        }

        // 무기 (불꽃비늘) 드랍
        if (dropGun01 == true)
        {
            GameObject gun01 = Instantiate(dropItems[3], gameObject.transform);
            gun01.transform.position = deadEnemyPos.position;
            gun01.SetActive(true);
        }

        // 무기 (지옥) 드랍
        if (dropGun02 == true)
        {
            GameObject gun02 = Instantiate(dropItems[4], gameObject.transform);
            gun02.transform.position = deadEnemyPos.position;
            gun02.SetActive(true);
        }
    }

    // 드랍될 아이템을 정한다.
    public void ChooseDropItems()
    {
        // [0] 코인은 무조건 드랍됨. 1/3 확률로 2배의 코인이 드랍되고, 1/3 확률로 3배의 코인이 드랍됨.
        // [1] 총알 1 드랍확률 1/3
        // [2] 총알 2 드랍확률 1/3
        // [3] 무기 (불꽃비늘) 드랍확률 1/10
        // [4] 무기 (지옥) 드랍확률 1/10

        // 코인 드랍
        int randomCoinDrop = Random.Range(0, 3);
        if (randomCoinDrop == 0) { coinCount = 1; }
        else if (randomCoinDrop == 1) { coinCount = 2; }
        else if (randomCoinDrop == 2) { coinCount = 3; }

        // 총알 1 드랍
        int randomBullet01Drop = Random.Range(0, 3);
        if (randomBullet01Drop == 0) { dropBullet01 = true; }
        else { dropBullet01 = false; }

        // 총알 2 드랍
        int randomBullet02Drop = Random.Range(0, 3);
        if (randomBullet02Drop == 0) { dropBullet02 = true; }
        else { dropBullet02 = false; }

        // 무기 (불꽃비늘) 드랍
        int randomGun01Drop = Random.Range(0, 20);
        if (randomGun01Drop == 0) { dropGun01 = true; }
        else { dropGun01 = false; }

        // 무기 (지옥) 드랍
        int randomGun02Drop = Random.Range(0, 20);
        if (randomGun02Drop == 0) { dropGun02 = true; }
        else { dropGun02 = false; }
    }
}
