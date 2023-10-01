using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionResetZone01 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 1)
            {
                other.transform.position = new Vector3(192, -23, 16);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 2)
            {
                other.transform.position = new Vector3(192, -23, 18);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 3)
            {
                other.transform.position = new Vector3(195, -23, 16);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 0)
            {
                other.transform.position = new Vector3(195, -23, 18);
            }
            else
            {
                other.transform.position = new Vector3(192, -23, 16);
            }
        }

    }
}
