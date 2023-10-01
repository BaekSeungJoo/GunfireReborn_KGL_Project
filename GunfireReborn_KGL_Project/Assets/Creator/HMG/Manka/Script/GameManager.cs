using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance
    {
        get
        {
            //만약 싱클턴 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;

        }
    }

    private static GameManager m_instance;

    public int nowStage = 0;        // 현재 스테이지가 몇인지
    public BlackSmithUI blackSmithUI; // 강화 횟수 초기화

    private void Awake()
    {
        if (instance != this)
        {
            //자신을 파괴
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 스테이지 인덱스 증가
        nowStage = 1;
    }
}