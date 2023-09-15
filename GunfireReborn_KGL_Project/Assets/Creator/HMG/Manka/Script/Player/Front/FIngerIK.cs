using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIngerIK : MonoBehaviourPun
{
    public Transform fingerBone;
    public Transform handBone;

    private float fingerRotationSpeed = 60f;
    private float handRotationSpeed = 120f;
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        // float fingerRotationAmount = fingerRotationSpeed;
        //fingerBone.Rotate(Vector3.right, fingerRotationAmount);

        float handRotationAmount =  handRotationSpeed;
        handBone.Rotate(Vector3.left, handRotationAmount);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
