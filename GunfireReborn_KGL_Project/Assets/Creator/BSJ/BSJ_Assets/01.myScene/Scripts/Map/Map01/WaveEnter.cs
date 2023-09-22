using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaveEnter : MonoBehaviourPun
{
    public GameObject waveFrontDoor;
    public GameObject wave;
    public bool isEnter;

    public void Update()
    {
        if (!isEnter)
        {
            return;
        }

        if(isEnter)
        {
            WaveEntered();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            photonView.RPC("WaveOpen", RpcTarget.All);
        }
    }

    [PunRPC]
    public void WaveOpen()
    {
        isEnter = true;
    }

    public void WaveEntered()
    {
        waveFrontDoor.SetActive(true);
        wave.SetActive(true);

        this.gameObject.SetActive(false);
    }
}
