using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Enemy enemy;
    private GameObject targetPlayer;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        targetPlayer = enemy.trackPlayer;
    }

    void Update()
    {
        //if(Camera.main != null)
        //{
        //    transform.LookAt(Camera.main.transform);
        //}

        if (targetPlayer != null) 
        {
            transform.LookAt(targetPlayer.transform.position);
        }
    }
}
