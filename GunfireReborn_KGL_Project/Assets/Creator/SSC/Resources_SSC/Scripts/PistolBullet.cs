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
    private Rigidbody myRigid = default;
    private float speed = 30.0f;


    public int bulletDamage = 5;

    private void OnEnable()
    {
        StartCoroutine(DestroyBullet(P_PoolObjType.PISTOLBULLET));
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            PhotonPoolManager.P_instance.CoolObj(this.gameObject, P_PoolObjType.PISTOLBULLET);



            EnemyHealth health = other.GetComponent<EnemyHealth>();

            photonView.RPC("ShotCallMaster_Basic", RpcTarget.MasterClient, health, bulletDamage);

        }

        if (other.CompareTag("LuckyShotPoint"))
        {
            PhotonPoolManager.P_instance.CoolObj(this.gameObject, P_PoolObjType.PISTOLBULLET);
            EnemyHealth health = GFunc.FindRootObj(other.gameObject).GetComponent<EnemyHealth>();


            photonView.RPC("ShotCallMaster_Lucky", RpcTarget.MasterClient, health, bulletDamage * 2);
        }

    }

    [PunRPC]
    public void ShotCallMaster_Basic(EnemyHealth health, int damage)
    {
        health.EnemyTakeDamage(damage);

    }

    [PunRPC]
    public void ShotCallMaster_Lucky(EnemyHealth health, int damage)
    {
        health.EnemyTakeDamage(damage * 2);
    }

    [PunRPC]
    public void Shot(Pistol_Lie001 lie)
    {
        myRigid.velocity = lie.transform.forward * speed;

    }
    private IEnumerator DestroyBullet(P_PoolObjType type)
    {
        yield return new WaitForSeconds(5f);
        PhotonPoolManager.P_instance.CoolObj(this.gameObject, type);
    }

}
