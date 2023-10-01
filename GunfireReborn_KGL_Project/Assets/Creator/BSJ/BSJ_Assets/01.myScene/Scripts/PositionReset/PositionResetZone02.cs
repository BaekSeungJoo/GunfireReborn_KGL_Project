using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionResetZone02 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 1)
            {
                other.transform.position = new Vector3(3, 0, 27);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 2)
            {
                other.transform.position = new Vector3(0, 0, 27);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 3)
            {
                other.transform.position = new Vector3(-3, 0, 27);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 0)
            {
                other.transform.position = new Vector3(-6, 0, 27);
            }
            else
            {
                other.transform.position = new Vector3(3, 0, 27);
            }
        }
    }
}
