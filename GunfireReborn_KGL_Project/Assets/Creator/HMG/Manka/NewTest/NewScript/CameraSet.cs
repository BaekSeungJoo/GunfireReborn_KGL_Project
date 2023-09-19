using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    //Near Clip Plane 0.52
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    [SerializeField]
    private CinemachineVirtualCamera theCamera;
    [SerializeField]
    private float lookSensitivity;

    void Start()
    {
        theCamera = FindObjectOfType<CinemachineVirtualCamera>();
        theCamera.transform.parent = gameObject.transform;
        Vector3 currentPos = gameObject.transform.position;
        theCamera.transform.localPosition = new Vector3(currentPos.x, currentPos.y + 0.8f, currentPos.z+0.22f);
    }


    void Update()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
     
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

        //float _yRotation = Input.GetAxisRaw("Mouse X");
    }
}
