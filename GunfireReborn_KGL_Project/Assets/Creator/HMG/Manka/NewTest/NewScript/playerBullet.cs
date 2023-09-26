using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class playerBullet : MonoBehaviourPun
{
    public int maxNBullet  = 675 ;
    public int remainNBullet;

    public int maxBBullet = 160;       
    public int remainBBullet = 10;
    private float activeFalseDistance = 2f;     // 해당 거리만큼 가까이가면 비활성화

    //private TMP_Text bulletText;

    // Start is called before the first frame update
    void Start()
    {
        remainBBullet = 90;
    }
    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        
        if (other.CompareTag("DropBigBullet"))
        {
            if (Vector3.Distance(transform.position, other.transform.position) < activeFalseDistance)
            {
                other.gameObject.SetActive(false);
                remainBBullet +=30;
                // 플레이어 쪽에서 해당 아이템을 먹었을 때 실행하는 로직
            }
        }


        if(other.CompareTag("DropNormalBullet"))
        {
            if (Vector3.Distance(transform.position, other.transform.position) < activeFalseDistance)
            {
                other.gameObject.SetActive(false);
                remainNBullet += 100;
                // 플레이어 쪽에서 해당 아이템을 먹었을 때 실행하는 로직
            }
        }

    }
}
