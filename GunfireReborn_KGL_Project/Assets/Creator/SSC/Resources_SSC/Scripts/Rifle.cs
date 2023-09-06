using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Transform muzzle;

    public float attackSpeed = 0.01f;
    private float attackTimer = 0f;

    private void Start()
    {
        muzzle = transform.Find("Muzzle").GetComponentInChildren<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        if(PlayerController.isShoot == true)
        {
            attackTimer += Time.deltaTime;

            if(attackTimer > attackSpeed)
            {
                Instantiate(bulletPrefab, muzzle.transform.position, transform.rotation);
                attackTimer = 0f;
            }
        }
    }
}
