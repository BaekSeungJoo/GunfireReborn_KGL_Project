using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackTest : MonoBehaviour
{
    Stack<int> test = new Stack<int>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            test.Push(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("다음 빼낼 데이터 조회 : " + test.Peek());
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            test.Pop();
        }
    }
}
