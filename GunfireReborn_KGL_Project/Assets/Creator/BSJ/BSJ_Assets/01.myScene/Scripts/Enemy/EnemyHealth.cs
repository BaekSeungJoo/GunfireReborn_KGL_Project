 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class EnemyHealth : MonoBehaviourPun
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


    [PunRPC]
    public void ApplyHealthUpdate(int newhealth, float newHpbar)
    {
        curHealth = newhealth;
        hpbar.fillAmount = newHpbar;

        if (curHealth <= 0)
        {
            // 우선 비활성화로 해놓았음.
            transform.gameObject.SetActive(false);

            // PhotonNetwork.Destroy(gameObject);
        }
    }

    
    // Enemy가 공격을 받았을 때 원격 프로시저 콜 해줄 함수
    public void EnemyTakeDamage(int _damage)
    {
        // 마스터클라이언트에서 입력받은 데미지에 따라 연산을 진행하고
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("입력받은 데미지 : " +  _damage);
            curHealth -= _damage;
            hpbar.fillAmount = (float)curHealth / (float)maxHealth;

            Debug.Log("몬스터 체력 : " + curHealth);

            // 연산된 감소된 체력과 체력바 값을 매개변수로 ApplyHealthUpdate() 메서드에 보내고 
            // 해당 작업을 나머지 클라이언트에서도 수행하게 한다.
            photonView.RPC("ApplyHealthUpdate", RpcTarget.Others, curHealth, hpbar.fillAmount);

            // 마찬가지로 나머지 클라이언트에서도 데미지를 받는 메소드를 수행 시킨다.
            // 하단에 현재체력이 0이하가 될시 게임오브젝트를 비활성화시키는 행동을 시켜주기 위해
            //photonView.RPC("EnemyTakeDamage", RpcTarget.Others, _damage);

        }

        if (curHealth <= 0)
        {
            // 우선 비활성화로 해놓았음.
            transform.gameObject.SetActive(false);

            // PhotonNetwork.Destroy(gameObject);
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
