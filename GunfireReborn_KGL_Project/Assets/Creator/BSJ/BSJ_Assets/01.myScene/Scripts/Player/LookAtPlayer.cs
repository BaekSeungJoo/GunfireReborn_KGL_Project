using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    void Update()
    {
        if (Camera.main != null)
        {
            //Vector3 targetDirection = Camera.main.transform.position - transform.position;
            //targetDirection.y = 0;
            //transform.rotation = Quaternion.LookRotation(targetDirection.normalized * -1, Vector3.up);

            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        }
    }
}
