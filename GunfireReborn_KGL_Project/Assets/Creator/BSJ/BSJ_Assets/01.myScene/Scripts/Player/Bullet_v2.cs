//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;
//using ExitGames.Client.Photon.StructWrapping;
//using Photon.Pun.UtilityScripts;
//using TMPro;

//public class Bullet_v2 : MonoBehaviour
//{
//    public float bulletSpeed = 50f;
//    public int damage = 30;
//    public float destroyTime = 5f;

//    public GameObject hitVFX;

//    private float _timer = 0f;
//    private PhotonView photonView;

//    public GameObject DamageTextPrefab;

//    private GameObject player = default;

//    private Vector3 hitVector;
//    private Vector3 hitVectorOffset = new Vector3(0, 0, 0);

//    public void Awake()
//    {
//        photonView = GetComponent<PhotonView>();
//        player = GameObject.FindWithTag("Player");
//    }

//    private void Update()
//    {
//        transform.Translate(0f, 0f, bulletSpeed * Time.deltaTime);

//        // 자동 총알 파괴
//        _timer += Time.deltaTime;

//        if(_timer > destroyTime)
//        {
//            if(PhotonNetwork.IsMasterClient)
//            {
//                photonView.RPC("DestroyBullet", RpcTarget.All, gameObject.name);
//            }
//        }
//    }

//    private void OnTriggerEnter(Collider collision)
//    {
//        if (PhotonNetwork.IsMasterClient)
//        {
//            // 총알 파괴
//            photonView.RPC("DestroyBullet", RpcTarget.All, gameObject.name);

//            // 이펙트 출력
//            PhotonNetwork.Instantiate(hitVFX.name, transform.position, Quaternion.identity);

//            // 데미지 텍스트 출력 위치
//            hitVector = transform.position + hitVectorOffset;

//            // 데미지 계산
//            BulletFire(collision);
//        }
//    }


//    public void BulletFire(Collider collision)
//    {
//        // 플레이어 타격
//        if (collision.transform.gameObject.GetComponent<Health>())
//        {
//            // 무기 데미지에 의한 점수
//            PhotonNetwork.LocalPlayer.AddScore(damage);

//            // 킬에 의한 점수
//            if (damage >= collision.transform.gameObject.GetComponent<Health>().health)
//            {
//                // kill
//                RoomManager.instance.kills++;
//                RoomManager.instance.SetHashes();

//                PhotonNetwork.LocalPlayer.AddScore(100);
//            }

//            collision.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
//        }

//        // 적 타격
//        else if (collision.transform.gameObject.GetComponent<EnemyHealth>())
//        {
//            collision.transform.gameObject.GetComponent<PhotonView>().RPC("EnemyTakeDamage", RpcTarget.All, damage);

//            // 데미지 텍스트 출력
//            PhotonNetwork.Instantiate(DamageTextPrefab.name, hitVector, Quaternion.identity).GetComponent<DamageText>().SetDamageText(damage, Color.white);
//        }

//        else if (collision.CompareTag("LuckyShotPoint"))
//        {
//            collision.transform.GetParentComponent<PhotonView>().RPC("EnemyTakeDamage", RpcTarget.All, damage * 2);

//            // 데미지 텍스트 출력
//            PhotonNetwork.Instantiate(DamageTextPrefab.name, hitVector, Quaternion.identity).GetComponent<DamageText>().SetDamageText(damage*2, Color.yellow);

//            Debug.Log("럭키샷");
//        }
//    }

//    [PunRPC]
//    private void DestroyBullet(string bulletName)
//    {
//        GameObject bullet = GameObject.Find(bulletName);
//        if (bullet != null)
//        {
//            PhotonNetwork.Destroy(bullet);
//        }
//    }
//}
