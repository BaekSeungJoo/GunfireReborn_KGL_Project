using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviourPun
{
    public int maxHealth;       //최대HP
    public int curHealth;       //현재HP

    public int ammo;            //전체탄창
    public int remainAmmo;      //현재탄창

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        //부활시 생기는 HP
        //현재HP를 MAXHP/2로 초기화
        curHealth = maxHealth/2;
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        //처음 시작 시 HP 
        //현재HP를 MAXHP로 초기화
        curHealth = maxHealth;
    }
    //player가 공격받았을때 원격 프로시저 콜 함수
    [PunRPC]
    public void playerTakeDamage(int _damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            curHealth -= _damage;

            if (curHealth <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

}
