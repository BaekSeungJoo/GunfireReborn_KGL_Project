using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using Photon.Pun;
using System;

public class Pistol_Lie001 : MonoBehaviourPun, IPunObservable
{
    // 총기의 현재 상태를 정의할 Enum : 발사가능, 탄창비어있음, 재장전
    public enum State { READY, EMPTY, RELOADING}
    public State state {  get; private set; }

    // 총알이 생성될 총구 위치
    private Transform muzzle;

    // 사격시 총구 화염 파티클
    public ParticleSystem muzzlFlash;

    private AudioSource fireSound;
    public AudioClip basicShot;
    public AudioClip EmptyMagAmmo;
    public AudioClip Pistol_Reload;

    // 사격 간격시간
    public float attackSpeed = 1f;
    private float attackTimer = 0f;

    // 탄창 최대 용량
    public int magCapacity = 9;
    // 탄창 현재 총알 수
    public int magAmmo;

    private WaitForSeconds reloadTime;

    public Vector3 clonePos = default;
    public Quaternion cloneRot = default;



    private void Start()
    {
        if(photonView.IsMine)
        {
            muzzle = transform.Find("Muzzle").GetComponentInChildren<Transform>();
        }

        fireSound = GetComponent<AudioSource>();
        reloadTime = new WaitForSeconds(0.7f);
        magAmmo = magCapacity;
        state = State.READY;
    }

    // 현재 총의 상태를 클론에게도 동기화 시켜주는 메소드, 상단에 인터페이스를 추가해야한다.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(magAmmo);
            stream.SendNext(state);
        }
        else
        {
            magAmmo = (int)stream.ReceiveNext();
            state = (State)stream.ReceiveNext();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            clonePos = transform.position;
            cloneRot = transform.rotation;

            if (state == State.EMPTY)
            {
                // 마우스 입력시 빈 탄창 소리 내기
                if (Input.GetMouseButtonDown(0))
                {
                    photonView.RPC("EmptyShot", RpcTarget.Others);
                }
                // 장전키를 눌렀을 시
                else if (Input.GetKeyDown(KeyCode.R) && magAmmo < magCapacity && state != State.RELOADING)
                {
                    state = State.RELOADING;
                    photonView.RPC("CloneReload", RpcTarget.All);
                }

                // 그 외에 상황에는 밑으로 진행하지 않는다 ( 공격 불가 )
                return;
            }

            // 탄창 용량 상관없이 장전을 누를때
            if (Input.GetKeyDown(KeyCode.R) && magAmmo < magCapacity && state != State.RELOADING)
            {
                photonView.RPC("CloneReload", RpcTarget.All);
            }


            if (state == State.READY)
            {
                if (magAmmo <= 0)
                {
                    magAmmo = 0;
                    photonView.RPC("ShotStop", RpcTarget.Others);
                    muzzlFlash.Stop();
                    state = State.EMPTY;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    // 내 총구가 바라보는 방향 저장
                    Vector3 foward = transform.right;
                    photonView.RPC("CloneShot", RpcTarget.Others, foward, muzzle.position, muzzle.rotation);
                    magAmmo -= 1;


                }
                else
                {
                    photonView.RPC("ShotStop", RpcTarget.Others);
                    muzzlFlash.Stop();
                }
            }

        }

    }


    [PunRPC]
    public void CloneShot(Vector3 foward, Vector3 Pos, Quaternion rot)
    {
        GameObject obj = PhotonPoolManager.P_instance.GetPoolObj(P_PoolObjType.PISTOLBULLET);
        Rigidbody objRigid = obj.GetComponent<Rigidbody>();

        obj.transform.position = Pos;
        obj.transform.rotation = rot;
        obj.gameObject.SetActive(true);

        objRigid.velocity = foward * 30f;

        muzzlFlash.Play();
        fireSound.clip = basicShot;
        fireSound.Play();
    }

    [PunRPC]
    public void ShotStop()
    {
        muzzlFlash.Stop();
    }

    [PunRPC]
    public void EmptyShot()
    {
        fireSound.clip = EmptyMagAmmo;
        fireSound.Play();
    }

    [PunRPC]
    public void CloneReload()
    {
        StartCoroutine(ReLoading());
    }
    IEnumerator ReLoading()
    {
        state = State.RELOADING;

        int reloadBullet = 0;
        reloadBullet = magCapacity - magAmmo;

        fireSound.clip = Pistol_Reload;
        fireSound.Play();

        yield return reloadTime;

        // 재장전 시간 이후 공격준비 상태로 바꾸며 코루틴 종료
        magAmmo += reloadBullet;
        state = State.READY;

    }


}
