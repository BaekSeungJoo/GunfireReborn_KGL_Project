using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class playerBullet : MonoBehaviourPun
{
    public int bullet  = 675 ;
    public int remainBullet = 30;
    
    public int sBullet = 45;
    public int remainSBullet = 4;   
    
    public int lBullet = 160;       
    public int remainlBullet = 10;  

    private int Gold;                //현재골드\
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
        if (Vector3.Distance(transform.position, other.transform.position) < activeFalseDistance)
        {
            Debug.Log("들어오고있는거야?");
            other.gameObject.SetActive(false);
            if (other.tag == "Coin")
            {
                Gold += 5;
            }
            // 플레이어 쪽에서 해당 아이템을 먹었을 때 실행하는 로직
        }

    }
}
