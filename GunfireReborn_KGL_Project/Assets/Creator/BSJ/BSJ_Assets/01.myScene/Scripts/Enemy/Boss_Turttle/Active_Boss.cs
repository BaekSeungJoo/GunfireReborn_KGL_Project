using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Active_Boss : MonoBehaviourPun
{
    public GameObject boss;
    public GameObject bossActiveEffect;
    public GameObject gameEndCheck;
    public GameObject bossCanavas;

    public bool isEnter = false;

    private void Update()
    {
        if (!isEnter)
        {
            return;
        }

        else if(isEnter)
        {
            boss.SetActive(true);
            bossActiveEffect.SetActive(true);
            gameEndCheck.SetActive(true);
            bossCanavas.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        photonView.RPC("CheckTriggerEnter", RpcTarget.All);
    }

    [PunRPC]
    public void CheckTriggerEnter()
    {
        isEnter = true;
    }
}
