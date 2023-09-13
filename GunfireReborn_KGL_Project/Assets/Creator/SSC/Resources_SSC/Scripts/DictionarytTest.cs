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

        
        foreach(string testkey in bullet.Keys)
        {
            Debug.Log("ÇöÀç µñ¼Å³Ê¸®¿¡ ´ã±ä Å° °ª : " + testkey);
        }

        foreach(Stack<int> stack in bullet.Values)
        {
            Debug.Log("µñ¼Å³Ê¸®¿¡ ´ã±ä ¹ë·ù°ª : " + stack);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
