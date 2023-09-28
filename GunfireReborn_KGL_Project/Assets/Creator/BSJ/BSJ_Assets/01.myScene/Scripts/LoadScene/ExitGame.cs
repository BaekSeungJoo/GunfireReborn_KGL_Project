using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();

                PhotonNetwork.LoadLevel("Main_TitleScene");
            }
        }
    }
}
