using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SetPlayerPosition_Map2 : MonoBehaviourPun
{
    private GameManager gameManager;

    private void Awake()
    {
        photonView.RPC("SetPlayerPos_Map2", RpcTarget.All);
    }

    [PunRPC]
    public void SetPlayerPos_Map2()
    {
        // 포톤 뷰를 들고 있는 플레이어를 찾아서 위치 동기화
        PhotonView[] allPhotonView = PhotonView.FindObjectsOfType<PhotonView>();
        List<PhotonView> players = new List<PhotonView>();

        // 포톤 뷰를 들고 있는 플레이어를 찾아서 위치 동기화
        foreach (PhotonView findPlayer in allPhotonView)
        {
            if (findPlayer.CompareTag("Player"))
            {
                players.Add(findPlayer);
            }
        }

        // 위치 초기화 
        // (3, 0, -3, -6) 4명의 x 값을 차례로 이것으로 줄 것임. y = -4, z = 27로 미리 좌표를 정리해두었음. 
        for(int i = 0; i < players.Count; i++)
        {
            if(i == 0)
            { players[i].transform.position = new Vector3(3, 0, 27); }
            else if(i == 1)
            { players[i].transform.position = new Vector3(0, 0, 27); }
            else if(i == 2)
            { players[i].transform.position = new Vector3(-3, 0, 27); }
            else if(i == 3)
            { players[i].transform.position = new Vector3(-6, 0, 27); }
        }
    }
}
