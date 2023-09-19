using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerReload : MonoBehaviourPun
{

    private Animator playerAnimator;
    private IK1 playerIk;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        playerAnimator = gameObject.GetComponent<Animator>();
        playerIk = gameObject.GetComponent<IK1>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (Input.GetButtonDown("Reload"))
        {
            Reload();
        }
    }

    private void Reload()
    {
        Debug.LogFormat("리로딩중~");
        //웨폰포지션의 자식오브젝트중 활성화된 무기를 찾아서 플레리어 손의 자식오브젝트로 만들어야한다.
        //그후 ik를 끈다음에 애니메이션을재생하도록한다.
        playerIk.enabled = false;
        playerAnimator.Play("Reloading", 1, 1);

        //그후에 ik를 다시키고 활성화된 무기를 다시원래의 웨폰위치로 되돌려야한다.
        //후....
    }
}
