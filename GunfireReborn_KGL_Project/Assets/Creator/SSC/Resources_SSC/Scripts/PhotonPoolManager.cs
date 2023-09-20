using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using Photon.Pun;
using System.Diagnostics.Tracing;

// ȣ���ϴ� ������Ʈ�� Ÿ���� �����ϱ����� �����ϴ� enum
public enum P_PoolObjType { BULLET, HELLBULLET, PISTOLBULLET }

// �ܺ� �ν�����â���� Ŭ���� ������ �����Ҽ� �ְ� ���ִ� [Serializable]
[Serializable]
public class P_PoolInfo
{
    // �ν�����â���� ������ ������
    public P_PoolObjType Type;    // ������Ʈ �̸� (Ÿ��), ȣ��������� ȣ���Ų ������Ʈ �̸� ����
    public int objAmount = 0;   // ������ Ǯ�� ������Ʈ ����
    public GameObject prefab;   // ������ Ǯ�� ������Ʈ ������
    public GameObject container;    // ������ Ǯ��������Ʈ�� ���� �����̳� ( ���� ������ƮǮ������ ���� �����ϱ� )
    public Stack<GameObject> poolObj = new Stack<GameObject>();     // ������ Ǯ�� ������Ʈ�� ��� �޸� Stack

}

// �ν�����â���� Ŭ���� ���ΰ� ���� ������ ���� [Serializable]
public class PhotonPoolManager : MonoBehaviourPun
{
    public static PhotonPoolManager P_instance;

    // ��ܿ� ������ PoolInfo Ŭ������ �ν�����â���� �����ϱ����� [Serializefield]
    // �ν�����â���� ������ ������ŭ PoolInfo�� Ŭ������ ���� List�� ���� �� �� 
    // == �� ����Ʈ�� �����ϴ� �ε��� �ȿ� ������ Stack�� ������ �� 
    [SerializeField]
    List<P_PoolInfo> poolList;


    private void Awake()
    {
        P_instance = this;
            // ������ ������Ʈ Ǯ ������ŭ ( ��ܿ� ������ ����Ʈ ) �ݺ�
        for(int i = 0; i < poolList.Count; i++)
        {
            // PoolInfo Ŭ������ ��Ƶ� ������ �� poolLsit�� ��´�.
            FillPool(poolList[i]);
            //photonView.RPC("FillPool", RpcTarget.AllBuffered, poolList[i]);
        }

    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if(PhotonNetwork.IsMasterClient)
    //    {
    //        stream.SendNext(poolList);
    //    }
    //    else
    //    {
    //        poolList = (List<P_PoolInfo>)stream.ReceiveNext();
    //    }
    //}

    // PoolInfo Ŭ���� ���ð� ( �ν�����â���� �����ϴ� �� ) ������� Ǯ��������Ʈ �����ϱ�
    void FillPool(P_PoolInfo poolInfo)
    {
        // PoolInfo Ŭ�������� ������ objAmount (������ ������Ʈ�� ����) ��ŭ �ݺ�
        for(int i = 0; i < poolInfo.objAmount ; i++)
        {
            // Ǯ���� ������Ʈ�� ���� ���� �ʱ�ȭ??
            GameObject tempObj = null;

            // �ν��Ͻ�ȭ ��Ų ������Ʈ ��Ƴֱ�
            tempObj = Instantiate(poolInfo.prefab, poolInfo.container.transform);
            tempObj.transform.position = poolInfo.container.transform.position;
            // ������ ������Ʈ ��Ȱ��ȭ, ��ġ �ʱ�ȭ, �޸� �Ҵ��ϱ�
            tempObj.SetActive(false);

            poolInfo.poolObj.Push(tempObj);
            //photonView.RPC("Push", RpcTarget.AllBuffered, tempObj, poolInfo);

        }

    }

    //[PunRPC]
    //public void Push(GameObject tempObj, P_PoolInfo poolInfo)
    //{
    //    tempObj.transform.parent = poolInfo.container.transform;

    //    // ������ ������Ʈ ��Ȱ��ȭ, ��ġ �ʱ�ȭ, �޸� �Ҵ��ϱ�
    //    tempObj.SetActive(false);
    //    tempObj.transform.position = poolInfo.container.transform.position;

    //    poolInfo.poolObj.Push(tempObj);
    //}

    // ������ Ǯ��������Ʈ�� ȣ���� �޼ҵ� 
    public GameObject GetPoolObj(P_PoolObjType type)
    {
        // GetPoolByType() �޼ҵ�� �����ϰ� ��ȯ���� type���� PoolInfo Ŭ������ �����ϱ�. 
        P_PoolInfo select = GetPoolByType(type);

        // �ش��ϴ� Ÿ���� ���� 
        Stack<GameObject> pool = select.poolObj;

        // ��Ƶ� ���ӿ�����Ʈ �ʱ�ȭ
        GameObject objInstance = default;

        // ȣ���ϴ� ������Ʈ ���� �����ص� ������Ʈ�� ����ϴٸ�
        if(pool.Count > 0)
        {
            // �ش� ������Ʈ�� ���ӿ�����Ʈ�� ���
            objInstance = pool.Peek();
            // Stack �޸𸮿��� ���ش�.
            pool.Pop();
        }
        // ȣ���ϴ� ������Ʈ ���� ���ð����� ���ٸ� 
        else
        {
            // Ǯ��������Ʈ�� ���� �������ش�.
            objInstance = Instantiate(select.prefab, select.container.transform);
            //objInstance.transform.parent = select.container.transform;
        }

        // ��� ������Ʈ ��ȯ
        return objInstance;
    }

    // ȣ��� Ǯ��������Ʈ�� Ǯ�� �ٽ� ��ȯ�ϴ� �޼ҵ�
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

    // Ǯ�� ������Ʈ ȣ��� ���� �Ǵ� Ǯ��������Ʈ�� �����س��� �޼ҵ�
    private P_PoolInfo GetPoolByType(P_PoolObjType type)
    {
        // ȣ���ϴ� ������Ʈ ������ �����ϴ� �ݺ���?
        // ������Ʈ Ǯ ������ŭ �ݺ����� ����
        for(int i = 0; i < poolList.Count; i++)
        {
            // ȣ���ϴ� ������Ʈ�� Ÿ�԰� ��ġ�Ѵٸ� 
            if(type == poolList[i].Type)
            {
                // �ش��ϴ� ������ƮǮ�� �ε����� ��ȯ�Ѵ�. (���� ����)
                return poolList[i];
            }
        }

        // �޼ҵ尡 �����Ǳ� ���� == �ݺ��� ���ο� ��ġ�ϴ� Ÿ���� ������Ʈ�� ���ٸ� null���� ��ȯ
        return null;
    }


}