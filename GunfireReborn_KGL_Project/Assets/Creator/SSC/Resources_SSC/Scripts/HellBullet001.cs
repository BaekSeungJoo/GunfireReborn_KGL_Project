using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HellBullet001 : MonoBehaviourPun
{
    private int bulletDamage = 3;
    private WaitForSeconds poolingTime;

    void Awake()
    {
        poolingTime = new WaitForSeconds(5f);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(DestroyBullet(P_PoolObjType.HELLBULLET));
    }
    private IEnumerator DestroyBullet(P_PoolObjType type)
    {
        yield return poolingTime;
        PhotonPoolManager.P_instance.CoolObj(this.gameObject, type);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            PhotonPoolManager.P_instance.CoolObj(this.gameObject, P_PoolObjType.PISTOLBULLET);
            EnemyHealth health = other.GetComponent<EnemyHealth>();

            health.EnemyTakeDamage(bulletDamage);
        }

        if (other.CompareTag("LuckyShotPoint"))
        {
            PhotonPoolManager.P_instance.CoolObj(this.gameObject, P_PoolObjType.PISTOLBULLET);
            EnemyHealth health = GFunc.FindRootObj(other.gameObject).GetComponent<EnemyHealth>();

            health.EnemyTakeDamage(bulletDamage);
        }
    }

}
