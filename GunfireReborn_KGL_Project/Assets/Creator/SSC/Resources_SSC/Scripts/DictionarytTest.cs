using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class DictionarytTest : MonoBehaviour
{

    public enum State { First, Second }
    public State state { get; set; }

    Stack<int> testStack = new Stack<int>();
    Stack<float> testStack2 = new Stack<float>();

    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, Stack<int>> bullet = new Dictionary<string, Stack<int>>();

        bullet.Add("testStack", testStack);

        for(int i = 0; i < 10; i++)
        {
            testStack.Push(i);
        }

        for(int i = 0;i < 10; i++)
        {
            Debug.Log("스택 데이터 값 : " + bullet.Keys);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
