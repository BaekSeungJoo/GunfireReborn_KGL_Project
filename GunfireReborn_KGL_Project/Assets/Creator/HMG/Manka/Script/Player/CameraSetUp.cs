using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetUp : MonoBehaviour
{

    public CinemachineVirtualCamera followCam;
    public GameObject weaponPosition;

    private float lookSensivility = 5f;
    private float cameraRotationLimit = 45;
    private float currentCameraRotationX;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement player = GetComponent<PlayerMovement>();
        followCam.transform.parent = player.transform;
        Vector3 CamPosition = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z+0.2f);
        followCam.transform.position = CamPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxis("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensivility;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        followCam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        weaponPosition.transform.localEulerAngles = new Vector3(currentCameraRotationX,0f, 0f);
    }
}
