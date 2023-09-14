using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Bullet001 : MonoBehaviour
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
        if (rifle.useSkiil == true)
        {
            Vector3 randomHit = cam.transform.forward * speed;
            Vector3 random = new Vector3
                (randomHit.x + Random.Range(randposMin, randposMax),
                randomHit.y + Random.Range(randposMin, randposMax),
                randomHit.z + Random.Range(randposMin, randposMax));
            myRigid.velocity = random;

            StartCoroutine(DestroyBullet(PoolObjType.BULLET));
            return;
        }

        myRigid.velocity = cam.transform.forward * speed;

        StartCoroutine(DestroyBullet(PoolObjType.BULLET));
    }

    private IEnumerator DestroyBullet(PoolObjType type)
    {
        yield return new WaitForSeconds(5f);
        ObjectPoolManager.instance.CoolObj(this.gameObject, type);
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyHealth helath = other.GetComponent<EnemyHealth>();

            if (helath != null)
            {
               helath.EnemyHpDown(bulletDamage);
            }
        }

        if(other.CompareTag("LuckyShotPoint"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();

            if(health != null)
            {
                health.EnemyHpDown(bulletDamage * 2);
            }
        }
    }


}
