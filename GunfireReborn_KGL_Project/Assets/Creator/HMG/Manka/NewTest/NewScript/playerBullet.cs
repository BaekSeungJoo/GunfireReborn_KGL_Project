using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class playerBullet : MonoBehaviourPun
{
    public int nBullet  = 675 ;
    public int remainNBullet = 30;

    public int bBullet = 160;       
    public int remainBBullet = 10;  

    private int Gold;                // 현재골드
    private float activeFalseDistance = 2f;     // 해당 거리만큼 가까이가면 비활성화
    private TMP_Text goldText;

    //private TMP_Text bulletText;

    // Start is called before the first frame update
    void Start()
    {
        goldText = GameObject.Find("CoinText").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
            goldText.text = Gold.ToString();

    }

    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // 일정거리 이상 들어가면 비활성화
        if (other.CompareTag("Coin"))
        {
            if (Vector3.Distance(transform.position, other.transform.position) < activeFalseDistance)
            {
                other.gameObject.SetActive(false);
                Gold += 5;
                // 플레이어 쪽에서 해당 아이템을 먹었을 때 실행하는 로직
            }
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
