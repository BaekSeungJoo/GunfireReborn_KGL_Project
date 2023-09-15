using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CubePosition : MonoBehaviourPun
{
    public CinemachineVirtualCamera Vcamera;
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            gameObject.transform.parent = Vcamera.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
