using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Photon.Pun;
using System.Runtime.Serialization;

public class PistolBullet : MonoBehaviourPun
{
    private int bulletDamage = 5;

    private WaitForSeconds poolingTime;

    private void Awake()
    {
        poolingTime = new WaitForSeconds(5f);
    }
    private void OnEnable()
    {
        // ========= 버그 테스트 하면서 몬스터에게 닿는순간 ( 오브젝트풀링이 되는 순간 ) 코루틴에 의해 꼬이는지 확인해야함 ===================
        StartCoroutine(DestroyBullet(P_PoolObjType.PISTOLBULLET));
        // ========= 버그 테스트 하면서 몬스터에게 닿는순간 ( 오브젝트풀링이 되는 순간 ) 코루틴에 의해 꼬이는지 확인해야함 ===================

    }


    public void OnTriggerEnter(Collider other)
    {
        GameObject obj = PhotonPoolManager.P_instance.GetPoolObj(P_PoolObjType.PISTOL_EFFECT);
        obj.transform.position = transform.position;
        
        if(other.CompareTag("Enemy"))
        {
            PhotonPoolManager.P_instance.CoolObj(this.gameObject, P_PoolObjType.PISTOLBULLET);
            EnemyHealth health = other.GetComponent<EnemyHealth>();

            health.EnemyTakeDamage(bulletDamage);

        }
        else if (other.CompareTag("LuckyShotPoint"))
        {
            PhotonPoolManager.P_instance.CoolObj(this.gameObject, P_PoolObjType.PISTOLBULLET);
            EnemyHealth health = GFunc.FindRootObj(other.gameObject).GetComponent<EnemyHealth>();

            health.EnemyTakeDamage(bulletDamage * 2);
        }

    }
    private IEnumerator DestroyBullet(P_PoolObjType type)
    {
        yield return poolingTime;
        PhotonPoolManager.P_instance.CoolObj(this.gameObject, type);
    }

}
