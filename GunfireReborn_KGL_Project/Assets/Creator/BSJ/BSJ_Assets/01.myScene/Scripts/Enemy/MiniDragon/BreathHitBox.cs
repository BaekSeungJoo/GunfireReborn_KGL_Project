using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BreathHitBox : MonoBehaviour
{
    // 오크 Enemy도 동일한 스크립트 사용함.

    public int damage;  // 몬스터의 데미지

    // 히트박스에 닿았다면
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어인지 탐색 && 이미 공격을 받았는지 체크
        if (other.CompareTag("Player"))
        {
            // 플레이어에게 데미지 주기

            // Health 스크립트에 있는 TakeDamage 메서드 RPC (remote procedure call)
            // other.transform.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
        }
    }
}
