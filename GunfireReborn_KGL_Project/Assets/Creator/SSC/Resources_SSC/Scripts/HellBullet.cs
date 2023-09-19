using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HellBullet : MonoBehaviour
{
    PlayerAttack1 shoot;
    public CinemachineVirtualCamera cam;
    private Rigidbody myRigid = default;
    private float speed = 30.0f;

    private float randposMin = -3.0f;
    private float randposMax = 3.0f;

    public int bulletDamage = 1;
   
    // Start is called before the first frame update
    void Awake()
    {
        cam = FindObjectOfType<CinemachineVirtualCamera>();
        shoot = FindObjectOfType<PlayerAttack1>();
        myRigid = GetComponent<Rigidbody>();

        Vector3 randomHit = cam.transform.forward * speed ;

        Vector3 random = new Vector3
            (randomHit.x + Random.Range(randposMin, randposMax),
            randomHit.y + Random.Range(randposMin, randposMax),
            randomHit.z + Random.Range(randposMin, randposMax));

        myRigid.velocity = random;
    }

    public void OnTriggerEnter(Collider other)
    {
            if(other.CompareTag("Enemy"))
            {
                EnemyTest helath = other.GetComponent<EnemyTest>();

                if (helath != null)
                {
                   helath.OnDamageable(bulletDamage);
                }
            }
    }


}
