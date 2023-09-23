using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
//using static UnityEditor.FilePathAttribute;

public class playerCure : MonoBehaviourPun
{
    //플레이어 상태를 가져오는 변수
    private playerHp hp;
  
    // Start is called before the first frame update
    void Start()
    {
        
        hp = GetComponent<playerHp>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

       

      
    }

   
}
