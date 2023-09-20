using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BreathHitBox : MonoBehaviourPun
{
    // 오크 Enemy도 동일한 스크립트 사용함.
    public int damage;  // 몬스터의 데미지

    // 히트박스에 닿았다면
    //private void OnTriggerEnter(Collider other)
    //{
    //    // 플레이어인지 탐색 && 이미 공격을 받았는지 체크
    //    if (other.CompareTag("Player"))
    //    {
    //        // 플레이어에게 데미지 주기
    //        playerHp player = other.GetComponent<playerHp>();
    //        photonView.RPC("MasterCall_Breath", RpcTarget.MasterClient, player, damage);
    //    }
    //}

    //[PunRPC]
    //public void MasterCall_Breath(playerHp player, int damage)
    //{
    //    player.PlayerTakeDamage(damage);
    //}
}
