//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;
//using Hashtable = ExitGames.Client.Photon.Hashtable;

//public class RoomManager : MonoBehaviourPunCallbacks
//{
//    public static RoomManager instance;

//    public GameObject player;

//    [Space]
//    public Transform[] spawnPoints;

//    [Space]
//    public GameObject roomCam;

//    [Space]
//    public GameObject nameUI;
//    public GameObject connectingUI;

//    private string nickname = "unnamed";

//    public string roomNameToJoin = "test";

//    [HideInInspector]
//    public int kills = 0;
//    [HideInInspector]
//    public int deaths = 0;

//    private void Awake()
//    {
//        instance = this;
//    }

//    public void ChangeNickName(string _name)
//    {
//        nickname = _name;
//    }

//    public void JoinRoomButtonPressed()
//    {
//        Debug.Log("Connecting");

//        PhotonNetwork.JoinOrCreateRoom("roomNameToJoin", null, null);

//        nameUI.SetActive(false);
//        connectingUI.SetActive(true);
//    }

//    // 룸을 생성을 사용자가 직접하므로 삭제
//    #region create room
//    //void Start()
//    //{

//    //}

//    //public override void OnConnectedToMaster()
//    //{
//    //    base.OnConnectedToMaster();

//    //    Debug.Log("Connected to Server");

//    //    PhotonNetwork.JoinLobby();
//    //}

//    //public override void OnJoinedLobby()
//    //{
//    //    base.OnJoinedLobby();

//    //    Debug.Log("We're in the lobby");

//    //    PhotonNetwork.JoinOrCreateRoom("roomNameToJoin", null, null);
//    //}
//    #endregion

//    public override void OnJoinedRoom()
//    {
//        base.OnJoinedRoom();

//        Debug.Log("We,re connected and in a room!");

//        roomCam.SetActive(false);

//        SpawnPlayer();
//    }

//    public void SpawnPlayer()
//    {
//        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

//        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
//        _player.GetComponent<PlayerSetUp>().IsLocalPlayer();
//        _player.GetComponent<Health>().isLocalPlayer = true;

//        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, nickname);
//        PhotonNetwork.LocalPlayer.NickName = nickname;
//    }

//    public void SetHashes()
//    {
//        try
//        {
//            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;

//            hash["kills"] = kills;
//            hash["deaths"] = deaths;

//            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
//        }
//        catch
//        {
//            // do nothing
//        }
//    }
//}
