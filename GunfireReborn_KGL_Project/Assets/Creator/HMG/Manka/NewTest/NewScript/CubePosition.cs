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
         Vcamera = FindObjectOfType<CinemachineVirtualCamera>();
         gameObject.transform.parent = Vcamera.transform;
         gameObject.transform.localPosition = Vector3.forward*3;

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
