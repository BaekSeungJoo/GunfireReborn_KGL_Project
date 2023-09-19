using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CubePosition1 : MonoBehaviourPun
{
    public Camera Vcamera;
    public GameObject cross;

    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            Vcamera = Camera.main;
            gameObject.transform.position = Vcamera.transform.position;
            gameObject.transform.parent = Vcamera.transform;
            return;
        }
        else
        {
            transform.position = cross.transform.position;
            transform.parent = cross.transform;
        }
       
    }
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if(stream.IsWriting)
    //    {
    //        stream.SendNext(transform.position);
    //    }
    //    else
    //    {
    //        transform.position = (Vector3)stream.ReceiveNext();
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
       
    }
}
