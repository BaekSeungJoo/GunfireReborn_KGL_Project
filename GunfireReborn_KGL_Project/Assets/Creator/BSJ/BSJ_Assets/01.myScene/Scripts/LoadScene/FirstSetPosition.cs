using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstSetPosition : MonoBehaviourPun
{
    private void Awake()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber % 4 == 1)
        {
            gameObject.transform.position = new Vector3(192, -23, 16);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 2)
        {
            gameObject.transform.position = new Vector3(192, -23, 18);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 3)
        {
            gameObject.transform.position = new Vector3(195, -23, 16);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 0)
        {
            gameObject.transform.position = new Vector3(195, -23, 18);
        }
        else
        {
            gameObject.transform.position = new Vector3(192, -23, 16);
        }

    }
}
