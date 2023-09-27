using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPhotonView : MonoBehaviourPun
{
    private void Update()
    {
        Debug.Log(PhotonView.Find(96));
        Debug.Log(PhotonView.Find(120));
        Debug.Log(PhotonView.Find(101));
        Debug.Log(PhotonView.Find(113));
        Debug.Log(PhotonView.Find(28));
        Debug.Log(PhotonView.Find(81));
        Debug.Log(PhotonView.Find(80));
    }
}
