using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager_SSC : MonoBehaviourPunCallbacks
{
    private string gameVersion = "3"; //게임버젼

    public Text connectionInfoText; //네트워크 정보를 표시할 텍스트
    public Button joinButton; //룸 접속 버튼
    // Start is called before the first frame update

    //게임 실행과 동시에 마스터 서버 접속 시도
    private void Start()
    {
        //접속에 필요한 게임버젼 설정
        PhotonNetwork.GameVersion = gameVersion;
        //설정한 정보로 마스터 서버 시도
        PhotonNetwork.ConnectUsingSettings();

        //롤 접속 버튼 잠시 비활성화
        joinButton.interactable = false;
        //접속 시도 중임을 텍스트로 표시
        connectionInfoText.text = "마스터 서버에 접속중 ...";
    }

    //마스터 서버 접속 성공 시 자동 실행
    public override void OnConnectedToMaster()
    {
        //룸 접속 버튼 활성화
        joinButton.interactable = true;
        //접속 정보 표시
        connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
    }

    //마스터 서버 접속 실패시 자동실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        //룸 접속 버튼 비활성화
        joinButton.interactable = false;
        //접속 정보 표시
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n 접속 재시도 중...";
        //마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    //룸 접속 시도 
    public void Connect()
    {
        //중복 접속 시도를 막기 위해 접속버튼 잠시 비활성화
        joinButton.interactable = false;

        //마스터서버에 접속중이라면
        if(PhotonNetwork.IsConnected)
        {
            //룸접속 실행
            connectionInfoText.text = "룸에 접속...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //마스터 서버에 접속 중이 아니라면 마스터 서버에 접속 시도
            connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지않음\n 접속 재시도중...";
            //마스터 서버로의 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
     
    }
    //빈방이 없어 랜덤 룸 참가에 실패한 경우 자동실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //접속 상태 표시
        connectionInfoText.text = "빈방이 없음, 새로운 방 생성...";
        //최대 4명을 수용 가능한 빈방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    //룸에 참가가 완료된 경우 자동실행
    public override void OnJoinedRoom()
    {
        //접속 상태 표시
        connectionInfoText.text = "방 참가 성공";
        //모든 룸 참가자가 Main씬을 로드하게 함
        PhotonNetwork.LoadLevel("Map_01_BSJ");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
