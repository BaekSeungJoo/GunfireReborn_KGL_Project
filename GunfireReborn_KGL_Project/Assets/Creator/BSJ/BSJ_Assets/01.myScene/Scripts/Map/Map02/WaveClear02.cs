using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaveClear02 : MonoBehaviourPun
{
    public GameObject NextWave;

    public bool waveClear = false;

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            // 자식오브젝트 중 하나라도 활성화가 되어있다면 클리어 실패
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                return;
            }
        }

        // 모든 자식 오브젝트가 비활성화 상태라면 클리어 성공
        photonView.RPC("CheckWaveClear", RpcTarget.All, true);

        if (waveClear)
        {
            NextWave.SetActive(true);
        }
    }

    [PunRPC]
    public void CheckWaveClear(bool _waveClear)
    {
        waveClear = _waveClear;
    }
}
