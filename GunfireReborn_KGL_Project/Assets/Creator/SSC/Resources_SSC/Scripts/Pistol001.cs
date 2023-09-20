using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class Pistol001 : MonoBehaviourPun
{
    // 총기의 현재 상태를 정의할 Enum : 발사가능, 탄창비어있음, 재장전
    public enum State { READY, EMPTY, RELOADING}
    public State state {  get; private set; }

    // 총알이 생성될 총구 위치
    private Transform muzzle;
    //private Transform lieMuzzle;

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
    public int magAmmo = 0;

    private WaitForSeconds reloadTime;

    private CinemachineVirtualCamera cam;

    void Start()
    {

        cam = FindObjectOfType<CinemachineVirtualCamera>();
        muzzle = transform.Find("Muzzle").GetComponentInChildren<Transform>();
        fireSound = GetComponent<AudioSource>();
        reloadTime = new WaitForSeconds(0.7f);

        magAmmo = magCapacity;
        state = State.READY;
        

    }
    // Update is called once per frame
    void Update()
    {
        if(transform.parent == null)
        {
            return;
        }

        if (state == State.EMPTY)
        {
            // 마우스 입력시 빈 탄창 소리 내기
            if (Input.GetMouseButtonDown(0))
            {
                fireSound.clip = EmptyMagAmmo;
                fireSound.Play();
            }
            // 장전키를 눌렀을 시
            else if (Input.GetKeyDown(KeyCode.R) && magAmmo < magCapacity && state != State.RELOADING)
            {
                state = State.RELOADING;
                fireSound.clip = Pistol_Reload;
                StartCoroutine(ReLoading());
            }

            // 그 외에 상황에는 밑으로 진행하지 않는다 ( 공격 불가 )
            return;
        }

        // 탄창 용량 상관없이 장전을 누를때
        if (Input.GetKeyDown(KeyCode.R) && magAmmo < magCapacity && state != State.RELOADING)
        {
            StartCoroutine(ReLoading());
        }


        if (state == State.READY)
        {
            if (magAmmo <= 0)
            {
                magAmmo = 0;
                muzzlFlash.Stop();
                state = State.EMPTY;
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject obj = null;
                Rigidbody objRigid = null;

                obj = PhotonPoolManager.P_instance.GetPoolObj(P_PoolObjType.PISTOLBULLET);

                if (obj != null)
                {
                    obj.transform.position = muzzle.transform.position;
                    obj.transform.rotation = muzzle.transform.rotation;

                    obj.gameObject.SetActive(true);

                    objRigid = obj.GetComponent<Rigidbody>();
                
                    objRigid.velocity = cam.transform.forward * 30f;
                }


                magAmmo -= 1;
                muzzlFlash.Play();
                fireSound.clip = basicShot;
                fireSound.Play();

            }
            else
            {
                muzzlFlash.Stop();
            }
        }
        // } 기본 사격 : 좌클릭

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
