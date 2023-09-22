using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnter02 : MonoBehaviourPun
{
    public GameObject wave;
    public bool isEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            photonView.RPC("WaveOpen", RpcTarget.All);
        }
    }

    public void Update()
    {
        if (!isEnter)
        {
            return;
        }

        if (isEnter)
        {
            WaveEntered();
        }
    }

    [PunRPC]
    public void WaveOpen()
    {
        isEnter = true;
    }

    public void WaveEntered()
    {
        wave.SetActive(true);

        this.gameObject.SetActive(false);
    }
}
