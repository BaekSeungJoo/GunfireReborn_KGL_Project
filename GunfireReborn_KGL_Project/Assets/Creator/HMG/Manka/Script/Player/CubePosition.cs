using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePosition : MonoBehaviour
{
    public CinemachineVirtualCamera Vcamera;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.parent = Vcamera.transform;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
