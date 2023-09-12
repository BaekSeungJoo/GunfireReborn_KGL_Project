using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Test : MonoBehaviour
{
    public GameObject prefab; // 재사용할 프리팹
    public int poolSize = 20; // 풀 크기
    private List<GameObject> objectPool; // 오브젝트 풀 리스트

    private void Start()
    {
        // 오브젝트 풀 초기화
        objectPool = new List<GameObject>();

        // 풀 사이즈 만큼 반복
        for(int i = 0; i < poolSize; i++)
        {
            // 프리팹 생성 (자식오브젝트로 생성)
            GameObject obj = Instantiate(prefab, this.transform);
            // 프리팹 비활성화
            obj.SetActive(false);
            // 오브젝트 풀 리스트에 프리팹 추가
            objectPool.Add(obj);
        }
    }

    // 오브젝트를 가져오는 함수
    public GameObject GetObjectFromPool()
    {
        // 오브젝트 풀 리스트에서 GameObject 요소를 모두 접근
        foreach(GameObject obj in objectPool)
        {
            // 풀안에 있는 프리팹 오브젝트가 활성화 되어있지 않다면
            if(obj.activeInHierarchy == false)
            {
                // 프리팹을 활성화 해주고 반환한다.
                obj.SetActive(true);
                return obj;
            }
        }

        // 풀에 사용 가능한 오브젝트가 없을 경우 새로 생성
        GameObject newObj = Instantiate(prefab);
        objectPool.Add(newObj); // 리스트에 새로운 프리팹 추가
        newObj.SetActive(true); // 새로운 프리팹 활성화
        return newObj;          // 새로운 프리팹 반환
    }

    // 오브젝트를 풀로 반환하는 함수
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
