using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[Serializable]
public class Test
{
    public int testAmount = 0;
    public GameObject prefab;
}

// 인스펙터창에서 클래스 내부값 조절 접근을 위한 [Serializable]
public class DictionarytTest : MonoBehaviour
{
    // 상단에 선언한 Test 클래스를 인스펙터창에서 접근하기위한 리스트 
    [SerializeField]
    List<Test> test;

    public GameObject[] prefabs;

    Stack<GameObject> stack = new Stack<GameObject>();
    Stack<GameObject> floatStack = new Stack<GameObject>();
    Stack<GameObject> stringStack = new Stack<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, Stack<GameObject>> dic = new Dictionary<string, Stack<GameObject>>();

        for(int i  = 0; i < 10; i++)
        {
            stack.Push(prefabs[0]);
            floatStack.Push(prefabs[1]);
            stringStack.Push(prefabs[2]);
        }   

        dic.Add("Cube", stack);
        dic.Add("Sphere", floatStack);
        dic.Add("Capsule", stringStack);

        Debug.Log("딕셔너리의 카운트값 :" + dic["Capsule"].Count);

        Debug.Log("===== 인스턴스화 전 =====");

        for(int i = 0; i < 10; i++)
        {
            Instantiate(prefabs[2], transform.position, transform.rotation);
            Debug.Log("딕셔너리의" + i + " 번째 카운트 값 : " + dic["Capsule"].Count);
            dic["Capsule"].Pop();
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
