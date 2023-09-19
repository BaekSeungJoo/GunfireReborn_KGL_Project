
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHp : MonoBehaviour
{
    public int maxHealth;       //최대HP
    public int curHealth;       //현재HP

    public int ammo;            //전체탄창
    public int remainAmmo;      //현재탄창

    private void Awake()
    {

        //부활시 생기는 HP
        //현재HP를 MAXHP/2로 초기화
        curHealth = maxHealth/2;
    }

    private void Start()
    {
        //처음 시작 시 HP 
        //현재HP를 MAXHP로 초기화
        curHealth = maxHealth;
    }
    //player가 공격받았을때 원격 프로시저 콜 함수
    public void playerTakeDamage(int _damage)
    {
        curHealth -= _damage;

        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

}
