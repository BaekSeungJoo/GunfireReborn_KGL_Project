
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class playerHp : MonoBehaviourPun
{
    public int maxHealth;       //최대HP
    public int curHealth;       //현재HP

    public int ammo;            //전체탄창
    public int remainAmmo;      //현재탄창

    public int Gold;            //현재골드

    public GameObject hpBar;

    private Animator animator;

    private playerCure cure;

    private IK1 ik;

    private CinemachineVirtualCamera virtualCam;
    public enum State
    {
        play,
        groggy,
        die
    }

    public State state;
    private void Start()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();  
        ik = GetComponent<IK1>();
        cure = GetComponent<playerCure>();
        animator = GetComponent<Animator>();
        //처음 시작 시 HP 
        //현재HP를 MAXHP로 초기화
        state = State.play;
        curHealth = maxHealth;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        //photonView.RPC("StateUpdate", RpcTarget.All);
    }

    //player가 공격받았을때 원격 프로시저 콜 함수
    public void PlayerTakeDamage(int damage)
    {
        curHealth -= damage;

        if (curHealth <= 0)
        {
            state = State.groggy;
            
            cure.enabled = true;
        }

        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("PlayerHealthUpdated", RpcTarget.Others, curHealth);
        }
    }

    [PunRPC]
    public void PlayerHealthUpdated(int newHealth)
    {
        curHealth = newHealth;
    }


    private void StateUpdate()
    {
        if (state == State.groggy)
        {
            hpBar.SetActive(true);
            animator.SetBool("groggy", true);
            ik.enabled = false;
            cure.enabled = true;
            virtualCam.Follow = gameObject.transform;
            virtualCam.LookAt = gameObject.transform;
        }
        else
        {
            hpBar.SetActive(false);
            animator.SetBool("groggy", false);
            virtualCam.Follow = null;
            virtualCam.LookAt = null;
        }
        if (state == State.die)
        {
            animator.SetTrigger("Dead");
            ik.enabled = false;
            cure.enabled = false;
        }
        if(state == State.play)
        {
            ik.enabled = true;
            cure.enabled = false;
        }

    }

}
