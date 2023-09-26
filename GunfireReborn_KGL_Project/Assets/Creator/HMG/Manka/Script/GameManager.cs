using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance
    {
        get 
        {
            //만약 싱클턴 변수에 아직 오브젝트가 할당되지 않았다면
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;

        }
    }

    private static GameManager m_instance;

    public GameObject playerPrefab;

    public int nowStage = 0;        // 현재 스테이지가 몇인지

    private void Awake()
    {
        if(instance != this)
        {
            //자신을 파괴
            Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo Info)
    {
      
    }
    // Start is called before the first frame update
    void Start()
    {
        // Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
        //위치값 y값은 0으로 변경
        // randomSpawnPos.y = 0f;

        //네트워크상의 모든 클라이언트에서 생성실행
        //해당 게임 오브젝트의 주도권은 생성 메서드를 직접 실행한 클라이언트에 있음
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);

        nowStage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("TestScene");
    }

}
