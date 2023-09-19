using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDropVelocity : MonoBehaviour
{
    private Rigidbody rb;
    private float randonPosX;
    private float randonPosY;
    private float randonPosZ;
    private Vector3 newVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Ground"))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnEnable()
    {
        SetVelocity(); 
        rb.AddForce(newVelocity, ForceMode.Impulse);
    }

    public void SetVelocity()
    {
        randonPosX = Random.Range(0f, 0.1f);
        randonPosY = Random.Range(0f, 0.5f);
        randonPosZ = Random.Range(0f, 0.1f);
        newVelocity = new Vector3(randonPosX, randonPosY, randonPosZ);
    }
}
