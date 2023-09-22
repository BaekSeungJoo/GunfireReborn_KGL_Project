using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    //private void Start()
    //{
    //    GameObject spawn = GameObject.FindWithTag("Spawn");

    //    transform.position = spawn.transform.position; 
    //}
}
