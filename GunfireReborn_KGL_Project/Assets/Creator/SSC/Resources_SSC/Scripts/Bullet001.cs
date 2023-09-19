using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Bullet001 : MonoBehaviourPun
{
    private CinemachineVirtualCamera cam;
    private CrimsonFirescale001 rifle;
    private Rigidbody myRigid = default;
    private float speed = 30.0f;

    private float randposMin = -3.0f;
    private float randposMax = 3.0f;

    public int bulletDamage = 4;
   
    // Start is called before the first frame update
    void Awake()
    {
        rifle = FindObjectOfType<CrimsonFirescale001>();
        cam = FindObjectOfType<CinemachineVirtualCamera>();
        myRigid = GetComponent<Rigidbody>();
     
    }

    private void OnEnable()
    {
        //if (rifle.useSkiil == true)
        //{
        //    Vector3 randomHit = cam.transform.forward * speed;
        //    Vector3 random = new Vector3
        //        (randomHit.x + Random.Range(randposMin, randposMax),
        //        randomHit.y + Random.Range(randposMin, randposMax),
        //        randomHit.z + Random.Range(randposMin, randposMax));
        //    myRigid.velocity = random;

        //    StartCoroutine(DestroyBullet(PoolObjType.BULLET));
        //    return;
        //}

        myRigid.velocity = cam.transform.forward * speed;

        StartCoroutine(DestroyBullet(P_PoolObjType.BULLET));
    }

    private IEnumerator DestroyBullet(P_PoolObjType type)
    {
        yield return new WaitForSeconds(5f);
        PhotonPoolManager.P_instance.CoolObj(this.gameObject, type);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            PhotonPoolManager.P_instance.CoolObj(this.gameObject, P_PoolObjType.PISTOLBULLET);
            EnemyHealth helath = other.GetComponent<EnemyHealth>();

            photonView.RPC("ShotCallMaster_Basic", RpcTarget.MasterClient, helath, bulletDamage);
        }

        if (other.CompareTag("LuckyShotPoint"))
        {
            PhotonPoolManager.P_instance.CoolObj(this.gameObject, P_PoolObjType.PISTOLBULLET);

            EnemyHealth helath = GFunc.FindRootObj(other.gameObject).GetComponent<EnemyHealth>();
            photonView.RPC("ShotCallMaster_Lucky", RpcTarget.MasterClient, helath, bulletDamage);
        }
    }

    [PunRPC]
    public void ShotCallMaster_Basic(EnemyHealth helth, int damage)
    {
        helth.EnemyTakeDamage(damage);
    }

    [PunRPC]
    public void ShotCallMaster_Lucky(EnemyHealth helth, int damage)
    {
        helth.EnemyTakeDamage(damage * 2);
    }


}
