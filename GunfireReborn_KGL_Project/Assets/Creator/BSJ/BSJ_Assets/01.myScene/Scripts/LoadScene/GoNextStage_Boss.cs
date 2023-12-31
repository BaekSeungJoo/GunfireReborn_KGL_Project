using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoNextStage_Boss : MonoBehaviourPun
{
    public GameObject[] playerPos;

    private Canvas MainCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            photonView.RPC("GoToBoss", RpcTarget.All);
        }
        else
        {
            Debug.Log(other.tag);
            return;
        }
    }

    [PunRPC]
    public void GoToBoss()
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
        for (int i = 0; i < players.Count; i++)
        {
            if (i == 0)
            { players[i].transform.position = playerPos[0].transform.position; }
            else if (i == 1)
            { players[i].transform.position = playerPos[1].transform.position; }
            else if (i == 2)
            { players[i].transform.position = playerPos[2].transform.position; }
            else if (i == 3)
            { players[i].transform.position = playerPos[3].transform.position; }
        }
    }
}
