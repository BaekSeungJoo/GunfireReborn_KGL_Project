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
    private LoadingManager LoadingManager;

    private string gameVersion = "3"; //게임버젼

    //게임 실행과 동시에 마스터 서버 접속 시도
    private void Start()
    {
        LoadingManager = GameObject.Find("@Managers").GetComponent<LoadingManager>();

        //접속에 필요한 게임버젼 설정
        PhotonNetwork.GameVersion = gameVersion;
        //설정한 정보로 마스터 서버 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    //마스터 서버 접속 성공 시 자동 실행
    public override void OnConnectedToMaster()
    {
        Connect();
    }

    //마스터 서버 접속 실패시 자동실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        //마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    //룸 접속 시도 
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
    //빈방이 없어 랜덤 룸 참가에 실패한 경우 자동실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //최대 4명을 수용 가능한 빈방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    //룸에 참가가 완료된 경우 자동실행
    public override void OnJoinedRoom()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Map_01_PSY");
        operation.allowSceneActivation = false;

        while (!operation.isDone && operation.allowSceneActivation == false)
        {
            yield return new WaitForSeconds(3f);

            operation.allowSceneActivation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}