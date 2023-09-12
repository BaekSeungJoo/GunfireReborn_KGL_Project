 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;       // 최대 Hp
    public int curHealth;       // 현재 Hp

    public Image hpbar;         // HP바 이미지

    private void Awake()
    {
        // 현재 Hp는 최대 Hp로 초기화
        curHealth = maxHealth;

        // HP바 초기화
        hpbar.fillAmount = (float)curHealth / (float)maxHealth;
    }


    // Enemy 스스로가 공격을 받았을 때 원격 프로시저 콜 해줄 함수
    [PunRPC]
    public void EnemyTakeDamage(int _damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            curHealth -= _damage;

            hpbar.fillAmount = (float)curHealth / (float)maxHealth;

            if (curHealth <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public void EnemyHpDown(int _damage)
    {
        curHealth -= _damage;

        hpbar.fillAmount = (float)curHealth / (float)maxHealth;

        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
