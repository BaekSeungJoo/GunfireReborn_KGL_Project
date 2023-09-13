using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviour
{
    // 생성할 적 프리팹
    public GameObject[] enemyPrefab; // 우선 두마리만 체크해 볼 것임.
    // 생성 간격
    public float spawnTimer = 5f;
    private float _timer = 0f;
    // 생성 위치
    public Transform[] spawnPoints;

    // 최종 생성 결정된 몬스터
    private GameObject spawnEnemy;

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > spawnTimer )
        {
            if(PhotonNetwork.IsMasterClient)
            {
                int ranEnemy = Random.Range(0, 5); // 일단 두마리만 체크
                string spawnEnemyName = (ranEnemy == 0) ? enemyPrefab[0].name : enemyPrefab[1].name;

                // PhotonView 컴포넌트 가져오기
                PhotonView photonView = GetComponent<PhotonView>();

                // 적 생성 동기화
                photonView.RPC("SpawnEnemy", RpcTarget.All, spawnEnemyName);
            }

            _timer = 0f;
        }
    }

    [PunRPC]
    private void SpawnEnemy(string enemyName)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            // 적 생성
            GameObject enemy = PhotonNetwork.Instantiate(enemyName, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        }
    }
}
