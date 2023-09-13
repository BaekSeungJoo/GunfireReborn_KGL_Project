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
        List<Stack> poolList = new List<Stack>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
