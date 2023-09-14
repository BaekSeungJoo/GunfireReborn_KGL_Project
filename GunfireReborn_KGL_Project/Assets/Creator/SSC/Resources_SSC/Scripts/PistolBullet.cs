using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private Rigidbody myRigid = default;
    private float speed = 30.0f;

    public int bulletDamage = 5;
   
    // Start is called before the first frame update
    void Awake()
    {
        cam = FindObjectOfType<CinemachineVirtualCamera>();
        myRigid = GetComponent<Rigidbody>();

    }

    private void OnEnable()
    {
        myRigid.velocity = cam.transform.forward * speed;

        StartCoroutine(DestroyBullet(PoolObjType.PISTOLBULLET));
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

        if (other.CompareTag("LuckyShotPoint"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();

            if (health != null)
            {
                health.EnemyHpDown(bulletDamage * 2);
            }
        }
    }


}
