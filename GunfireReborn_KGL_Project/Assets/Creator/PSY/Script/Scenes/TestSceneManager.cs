using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TestSceneManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "3"; //게임버젼

    private void Start()
    {
        #region Photon : 게임 실행과 동시에 마스터 서버 접속 시도
        //접속에 필요한 게임버젼 설정
        PhotonNetwork.GameVersion = gameVersion;
        //설정한 정보로 마스터 서버 시도
        PhotonNetwork.ConnectUsingSettings();
        #endregion
    }

    #region Photon
    /// <summary>
    /// 마스터 서버 접속 성공 시 자동 실행
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Connect();
    }

    /// <summary>
    /// 마스터 서버 접속 실패시 자동실행
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
        //마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    ///  룸 접속 시도 
    /// </summary>
    public void Connect()
    {
        //마스터서버에 접속중이라면
        if (PhotonNetwork.IsConnected)
        {
            //룸접속 실행
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //마스터 서버로의 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    /// <summary>
    /// 빈방이 없어 랜덤 룸 참가에 실패한 경우 자동실행
    /// </summary>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //최대 4명을 수용 가능한 빈방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    /// <summary>
    /// 룸에 참가가 완료된 경우 자동실행
    /// </summary>
    public override void OnJoinedRoom()
    {
        StartCoroutine(LoadScene());
    }
    #endregion

    #region 비동기 로딩
    /// <summary>
    /// 비동기 로딩 함수
    /// </summary>
    private IEnumerator LoadScene()
    {
        // "Map_01_PSY" 씬을 비동기 작업으로 로드한다.
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main_Map_01");  
        operation.allowSceneActivation = false;  // 씬을 로드하는데 준비가 안됨.

        while (!operation.isDone && operation.allowSceneActivation == false)  // 씬의 로드가 끝날 때까지 반복
        {
            yield return new WaitForSeconds(3f);    // 실제 시간 3초 딜레이를 준다.

            operation.allowSceneActivation = true;  // 씬 로드의 준비를 끝낸다.
        }
    }
    #endregion
}