using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody myRigid = default;
    private float speed = 30f;
    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();

        Vector3 target = PlayerController.hitPoint - transform.position;

        myRigid.velocity = target.normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
