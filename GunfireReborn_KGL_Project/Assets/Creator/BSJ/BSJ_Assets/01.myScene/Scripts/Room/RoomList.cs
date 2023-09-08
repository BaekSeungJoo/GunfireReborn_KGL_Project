//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;
//using TMPro;

//public class RoomList : MonoBehaviourPunCallbacks
//{
//    public static RoomList instance;

//    public GameObject roomManagerGameObject;
//    public RoomManager roomManager;

//    [Header("UI")]
//    public Transform roomListParent;
//    public GameObject roomListItemPrefab;

//    private List<RoomInfo> cacheRoomList = new List<RoomInfo>();

//    public void ChangeRoomToCreatName(string _roomname)
//    {
//        roomManager.roomNameToJoin = _roomname;
//    }

//    private void Awake()
//    {
//        instance = this;
//    }

//    IEnumerator Start()
//    {
//        // precoutions
//        if(PhotonNetwork.InRoom)
//        {
//            PhotonNetwork.LeaveRoom();
//            PhotonNetwork.Disconnect();
//        }

//        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

//        PhotonNetwork.ConnectUsingSettings();
//    }

//    public override void OnConnectedToMaster()
//    {
//        base.OnConnectedToMaster();

//        PhotonNetwork.JoinLobby();
//    }

//    public override void OnRoomListUpdate(List<RoomInfo> roomList)
//    {
//        if (cacheRoomList.Count <= 0)
//        {
//            cacheRoomList = roomList;
//        }
//        else
//        {
//            foreach (var room in roomList)
//            {
//                for (int i = 0; i < cacheRoomList.Count; i++)
//                {
//                    if (cacheRoomList[i].Name == room.Name) 
//                    {
//                        List<RoomInfo> newList = cacheRoomList;

//                        if (room.RemovedFromList)
//                        {
//                            newList.Remove(newList[i]);
//                        }
//                        else
//                        {
//                            newList[i] = room;
//                        }

//                        cacheRoomList = newList;
//                    }
//                }
//            }
//        }

//        UpdateUI();
//    }


//    void UpdateUI()
//    {
//        foreach(Transform roomItem in roomListParent)
//        {
//            Destroy(roomItem.gameObject);
//        }

//        foreach(var room in cacheRoomList)
//        {
//            GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);

//            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
//            roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + " / 16";

//            roomItem.GetComponent<RoomItemButton>().RoomName = room.Name;
//        }
//    }

//    public void JoinRoomByName(string _name)
//    {
//        roomManager.roomNameToJoin = _name;
//        roomManagerGameObject.SetActive(true);
//        gameObject.SetActive(false);
//    }
//}
