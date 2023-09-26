using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontSet : MonoBehaviour
{
    private Camera camera;
    //private CinemachineVirtualCamera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        //camera = FindObjectOfType<CinemachineVirtualCamera>();
        gameObject.transform.position = new Vector3(camera.transform.position.x + 0.27f, camera.transform.position.y - 1.3f, camera.transform.position.z + 0.2f);
    }
}
