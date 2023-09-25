using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoru : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
