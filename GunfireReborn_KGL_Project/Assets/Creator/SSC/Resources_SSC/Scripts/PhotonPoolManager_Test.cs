using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using Photon.Pun;
using System.Diagnostics.Tracing;

// 호출하는 오브젝트의 타입을 구별하기위해 선언하는 enum

// 외부 인스펙터창에서 클래스 정보에 접근할수 있게 해주는 [Serializable]

// 인스펙터창에서 클래스 내부값 조절 접근을 위한 [Serializable]
public class PhotonPoolManager_Test : MonoBehaviourPun, IPunObservable
{
    public enum P_PoolObjTypeTest { BULLET, HELLBULLET, PISTOLBULLET }

    [Serializable]
    public class P_PoolInfo
    {
        // 인스펙터창에서 보여줄 정보들
        public P_PoolObjType Type;    // 오브젝트 이름 (타입), 호출시켰을때 호출시킨 오브젝트 이름 대조
        public int objAmount = 0;   // 생성할 풀링 오브젝트 갯수
        public GameObject prefab;   // 생성할 풀링 오브젝트 프리팹
        public GameObject container;    // 생성한 풀링오브젝트를 담을 컨테이너 ( 다중 오브젝트풀링으로 인한 구별하기 )
        public Stack<GameObject> poolObj = new Stack<GameObject>();     // 생성된 풀링 오브젝트가 담길 메모리 Stack

    }
    // 상단에 선언한 PoolInfo 클래스를 인스펙터창에서 접근하기위한 [Serializefield]
    // 인스펙터창에서 설정한 갯수만큼 PoolInfo의 클래스를 가진 List가 생성 될 것 
    // == 각 리스트에 대응하는 인덱스 안에 개별적 Stack이 생성될 것 
    [SerializeField]
    List<P_PoolInfo> poolList;

    private void Awake()
    {
        // 생성할 오브젝트 풀 갯수만큼 ( 상단에 생성한 리스트 ) 반복
        for(int i = 0; i < poolList.Count; i++)
        {
            // PoolInfo 클래스에 담아둔 정보를 각 poolLsit에 담는다.
            FillPool(poolList[i]);
            //photonView.RPC("FillPool", RpcTarget.Others, poolList);
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    // PoolInfo 클래스 셋팅값 ( 인스펙터창에서 셋팅하는 것 ) 기반으로 풀링오브젝트 생성하기
    void FillPool(P_PoolInfo poolInfo)
    {
        // PoolInfo 클래스에서 세팅한 objAmount (생성할 오브젝트의 갯수) 만큼 반복
        for(int i = 0; i < poolInfo.objAmount ; i++)
        {
            // 풀링할 오브젝트를 담을 변수 초기화??
            GameObject tempObj = null;

            // 인스턴스화 시킨 오브젝트 담아넣기
            tempObj = PhotonNetwork.Instantiate(poolInfo.prefab.name, poolInfo.container.transform.position, Quaternion.identity);
            tempObj.transform.parent = poolInfo.container.transform;

            // 생성한 오브젝트 비활성화, 위치 초기화, 메모리 할당하기
            tempObj.SetActive(false);
            tempObj.transform.position = poolInfo.container.transform.position;

            poolInfo.poolObj.Push(tempObj);
        }

    }

    // 생성한 풀링오브젝트를 호출할 메소드 
    public GameObject GetPoolObj(P_PoolObjType type)
    {
        // GetPoolByType() 메소드로 검출하고 반환받은 type값을 PoolInfo 클래스와 대조하기. 
        P_PoolInfo select = GetPoolByType(type);

        // 해당하는 타입의 스택 
        Stack<GameObject> pool = select.poolObj;

        // 담아둘 게임오브젝트 초기화
        GameObject objInstance = default;

        // 호출하는 오브젝트 수가 세팅해둔 오브젝트로 충분하다면
        if(pool.Count > 0)
        {
            // 해당 오브젝트를 게임오브젝트에 담고
            objInstance = pool.Peek();
            // Stack 메모리에서 빼준다.
            pool.Pop();
        }

        // 호출하는 오브젝트 수가 세팅값보다 많다면 
        else
        {
            // 풀링오브젝트를 새로 생성해준다.
            objInstance = PhotonNetwork.Instantiate(select.prefab.name, select.container.transform.position, Quaternion.identity);
            objInstance.transform.parent = select.container.transform;
        }

        // 담긴 오브젝트 반환
        return objInstance;
    }

    // 호출된 풀링오브젝트를 풀에 다시 반환하는 메소드
    public void CoolObj(GameObject obj, P_PoolObjType type)
    {
        P_PoolInfo select = GetPoolByType(type);

        obj.SetActive(false);
        obj.transform.position = select.container.transform.position;
        Stack<GameObject> pool = select.poolObj;

        if(pool.Contains(obj) == false)
        {
            pool.Push(obj);
        }
    }

    // 풀링 오브젝트 호출시 대응 되는 풀링오브젝트를 검출해내는 메소드
    private P_PoolInfo GetPoolByType(P_PoolObjType type)
    {
        // 호출하는 오브젝트 종류를 검출하는 반복문?
        // 오브젝트 풀 갯수만큼 반복문을 돌려
        for(int i = 0; i < poolList.Count; i++)
        {
            // 호출하는 오브젝트의 타입과 일치한다면 
            if(type == poolList[i].Type)
            {
                // 해당하는 오브젝트풀의 인덱스를 반환한다. (종류 구별)
                return poolList[i];
            }
        }

        // 메소드가 성립되기 위한 == 반복문 내부에 일치하는 타입의 오브젝트가 없다면 null값을 반환
        return null;
    }


}
