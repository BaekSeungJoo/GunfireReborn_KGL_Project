using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Cinemachine;
using Photon.Pun;


// 총기의 현재 상태를 정의할 Enum : 발사가능, 탄창비어있음, 재장전 중
public class CrimsonFirescale_Lie001 : MonoBehaviourPun, IPunObservable
{
    public bool useSkiil = false;

    public enum State { READY, EMPTY, RELOADING }
    public State state {  get; private set; }

    public ParticleSystem muzzlFlash;
    private Transform muzzle;
    private AudioSource fireSound;
    public AudioClip basicShot;
    public AudioClip skillShot;
    public AudioClip EmptyMagAmmo;
    public AudioClip CrimsonFirescale_Reload;

    // 사격 간격시간
    private float attackSpeed = 0.1f;    
    private float attackTimer = 0f;    

    // 전체 최대 총알 수
    private int maxAmmoRemain = 90;
    // 남아있는 전체 총알 수
    private int ammoRemain;

    // 탄창 최대 용량
    private int magCapacity = 30;
    // 탄창 현재 총알 수
    private int magAmmo;

    private int skillAmmo = 4;

    private float bulletSpeed = 50f;

    private WaitForSeconds reloadTime;

    private float xMax = 0.1f;
    private float xMin = -0.1f;

    private float yMax = 0.1f;
    private float yMin = -0.1f;

    private float zMax = 0.1f;
    private float zMin = -0.1f;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
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

    private void Start()
    {
        muzzle = transform.Find("Muzzle").GetComponentInChildren<Transform>();
        fireSound = GetComponent<AudioSource>();
        reloadTime = new WaitForSeconds(2f);
        ammoRemain = maxAmmoRemain;
        magAmmo = magCapacity;


        state = State.READY;        

    }
    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            if (transform.parent == null)
            {
                return;
            }

            // 탄창이 비어있는 상태라면
            if (state == State.EMPTY)
            {
                // 마우스 입력시 빈 탄창 소리 내기
                if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    photonView.RPC("EmptyShot", RpcTarget.Others);
                }
                // 장전키를 눌렀을 시
                else if (Input.GetKeyDown(KeyCode.R) && state != State.RELOADING)
                {
                    StartCoroutine(ReLoading());
                    photonView.RPC("ReLoading_Pun", RpcTarget.Others);
                }

            }

            // 탄창 용량 상관없이 장전을 누를때
            if(Input.GetKeyDown(KeyCode.R) && state != State.RELOADING)
            {
                StartCoroutine(ReLoading());
                photonView.RPC("ReLoading_Pun", RpcTarget.Others);
            }

            if(state == State.READY)
            {
                if(magAmmo <= 0)
                {
                    magAmmo = 0;
                    state = State.EMPTY;
                    photonView.RPC("ShotStop", RpcTarget.Others);
                }

                if (Input.GetMouseButton(0) == true)
                {
                    attackTimer += Time.deltaTime;

                    if(attackTimer > attackSpeed)
                    {
                        photonView.RPC("CloneShot", RpcTarget.Others, transform.right, muzzle.transform.position, muzzle.transform.rotation
                            , UpgradeManager.up_Instance.rifleDamage);
                        attackTimer = 0f;
                        magAmmo -= 1;                   
                    }
                }
                else
                {
                    photonView.RPC("ShotStop", RpcTarget.Others);
                }
                // } 기본 사격 : 좌클릭

                if(Input.GetMouseButtonDown(1))
                {
                    Vector3 muzzleFoward = muzzle.transform.forward;

                    // 남아있는 탄창이 스킬용량보다 작을 때 
                    if (magAmmo < skillAmmo)
                    {
                        for (int i = 0; i < magAmmo; i++)
                        {   
                            muzzleFoward.x = muzzleFoward.x + Random.Range(xMax, xMin);
                            muzzleFoward.y = muzzleFoward.y + Random.Range(yMax, yMin);
                            muzzleFoward.z = muzzleFoward.z + Random.Range(xMax, xMin);
                            photonView.RPC("UsingSkill", RpcTarget.Others, muzzleFoward, muzzle.transform.position, muzzle.transform.rotation
                                , UpgradeManager.up_Instance.rifleDamage);
                            magAmmo -= magAmmo;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < skillAmmo; i++)
                        {
                            muzzleFoward.x = muzzleFoward.x + Random.Range(xMax, xMin);
                            muzzleFoward.y = muzzleFoward.y + Random.Range(yMax, yMin);
                            muzzleFoward.z = muzzleFoward.z + Random.Range(xMax, xMin);
                            photonView.RPC("UsingSkill", RpcTarget.Others, muzzleFoward, muzzle.transform.position, muzzle.transform.rotation
                                ,UpgradeManager.up_Instance.rifleDamage);
                            magAmmo -= 1;
                        }
                    }

                }

            }

        }

    }


    [PunRPC]
    public void UsingSkill(Vector3 foward, Vector3 Pos, Quaternion rot, int damage)
    {
        GameObject obj = null;
        Rigidbody objRigid = null;
        Bullet001 objDamage;
        
        obj = PhotonPoolManager.P_instance.GetPoolObj(P_PoolObjType.BULLET);

        if(obj != null)
        {
            obj.transform.position = Pos;
            obj.transform.rotation = rot;
        
            objRigid = obj.GetComponent<Rigidbody>();
            objDamage = obj.GetComponent<Bullet001>();
        
            objDamage.riflebulletDamage = damage;
            obj.gameObject.SetActive(true);
            objRigid.velocity = foward* bulletSpeed;

        }


        muzzlFlash.Play();
        fireSound.clip = skillShot;
        fireSound.volume = 0.8f;
        fireSound.Play();

    }

    [PunRPC]
    public void ReLoading_Pun()
    {
        fireSound.clip = CrimsonFirescale_Reload;
        fireSound.Play();

    }

    IEnumerator ReLoading()
    {
        state = State.RELOADING;

        int reloadBullet = 0;

        reloadBullet = magCapacity - magAmmo;


        if(reloadBullet > ammoRemain)
        {
            magAmmo += ammoRemain;
            ammoRemain = 0;

            yield return reloadTime;

            // 재장전 시간 이후 공격준비 상태로 바꾸며 코루틴 종료
            state = State.READY;

            yield break;
        }
        // 재장전 시간
        yield return reloadTime;
            
        ammoRemain -= reloadBullet;
        magAmmo += reloadBullet;

        // 재장전 시간 이후 공격준비 상태로 바꾸며 코루틴 종료
        state = State.READY;

        yield break;
    }

    [PunRPC]
    public void CloneShot(Vector3 foward, Vector3 Pos, Quaternion rot, int damage)
    {
        GameObject obj = null;
        Rigidbody objRigid = null;
        Bullet001 objDamage;

        obj = PhotonPoolManager.P_instance.GetPoolObj(P_PoolObjType.BULLET);

        if (obj != null)
        {
            obj.transform.position = Pos;
            obj.transform.rotation = rot;

            objRigid = obj.GetComponent<Rigidbody>();
            objDamage = obj.GetComponent<Bullet001>();

            obj.gameObject.SetActive(true);
            objDamage.riflebulletDamage = damage;
            objRigid.velocity = foward * bulletSpeed;

        }

        muzzlFlash.Play();
        fireSound.clip = basicShot;
        fireSound.volume = 0.4f;
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

}
