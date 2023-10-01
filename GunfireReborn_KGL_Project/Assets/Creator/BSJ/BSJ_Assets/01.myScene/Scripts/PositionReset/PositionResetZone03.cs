using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionResetZone03 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 1)
            {
                other.transform.position = new Vector3(-30, 6, -32);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 2)
            {
                other.transform.position = new Vector3(-32, 6, -32);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 3)
            {
                other.transform.position = new Vector3(-34, 6, -32);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 4 == 0)
            {
                other.transform.position = new Vector3(-36, 6, -32);
            }
            else
            {
                other.transform.position = new Vector3(-36, 6, -32);
            }
        }
    }
}
