//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;

//public class AttackHitBox : MonoBehaviour
//{
//    public int damage;                  // 몬스터의 데미지
//    private bool isAttcked = false;     // 연속적으로 데미지를 주는 것을 방지하기 위한 조건

//    // 히트박스에 닿았다면
//    private void OnTriggerEnter(Collider other)
//    {
//        // 플레이어인지 탐색 && 이미 공격을 받았는지 체크
//        if(other.CompareTag("Player") && isAttcked == false)
//        {
//            // 플레이어에게 데미지 주기
//            Debug.Log("공격당함" + damage);
            
//            // Health 스크립트에 있는 TakeDamage 메서드 RPC (remote procedure call)
//            other.transform.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);

//            // 공격을 했음을 체크 (데미지가 중복으로 들어가는 것 방지)
//            isAttcked = true;
//        }
//    }

//    // 히트박스가 사라질 때 isAttcked 초기화
//    private void OnDisable()
//    {
//        isAttcked = false;
//    }
//}
