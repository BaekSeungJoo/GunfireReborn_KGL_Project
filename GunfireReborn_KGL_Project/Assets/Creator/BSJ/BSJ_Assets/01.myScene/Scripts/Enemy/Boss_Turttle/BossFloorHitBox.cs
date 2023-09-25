using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFloorHitBox : MonoBehaviourPun
{
    public int damage;                  // 몬스터의 데미지
    private bool isAttcked = false;     // 연속적으로 데미지를 주는 것을 방지하기 위한 조건

    // 히트박스에 닿았다면
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어인지 탐색 && 이미 공격을 받았는지 체크
        if (other.CompareTag("Player") && isAttcked == false)
        {
            // 플레이어에게 데미지 주기
            playerHp player = other.GetComponent<playerHp>();
            photonView.RPC("MasterCall", RpcTarget.MasterClient, player, damage);

            // 공격을 했음을 체크 (데미지가 중복으로 들어가는 것 방지)
            isAttcked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isAttcked = false;
    }

    [PunRPC]
    public void MasterCall(playerHp player, int damage)
    {
        player.PlayerTakeDamage(damage);
    }

    // 히트박스가 사라질 때 isAttcked 초기화
    private void OnDisable()
    {
        isAttcked = false;
    }
}
