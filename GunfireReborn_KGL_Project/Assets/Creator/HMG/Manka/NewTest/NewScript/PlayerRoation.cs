using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerRoation : MonoBehaviourPun
{

    [SerializeField]
    private float lookSensitivity;

    private Rigidbody playerRB;
    // Start is called before the first frame update
    void Start()
    {
       playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        CharacterRotate();
    }

    private void CharacterRotate()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        playerRB.MoveRotation(playerRB.rotation * Quaternion.Euler(_characterRotationY));
    }
}
