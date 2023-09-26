using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoNextLoadingScene : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            photonView.RPC("LoadNextScene_Map3", RpcTarget.All);
        }
        else
        {
            Debug.Log(other.tag);
            return;
        }
    }

    [PunRPC]
    public void LoadNextScene_Map3()
    {
        SceneManager.LoadScene("Main_LoadingScene");
    }
}
