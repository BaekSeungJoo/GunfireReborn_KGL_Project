using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Cinemachine;
using System.Security.Cryptography;
using UnityEngine.UI;


// 총기의 현재 상태를 정의할 Enum : 발사가능, 탄창비어있음, 재장전 중
public class CrimsonFirescale001 : MonoBehaviour
{
    public bool useSkiil = false;

    public enum State { READY, EMPTY, RELOADING }
    public State state {  get; private set; }

    private CinemachineVirtualCamera cam;

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

    playerBullet bulletInfo;

    [SerializeField] private GameObject NBullet;
    [SerializeField] private GameObject NBulletBack;
    [SerializeField] private TextMeshProUGUI BulletText;
    [SerializeField] private GameObject BlackSmith;

    Image NbulletFill;

    private void Awake()
    {
        bulletInfo = transform.parent.GetComponent<playerBullet>();
        magAmmo = magCapacity;

        NbulletFill = NBullet.GetComponent<Image>();

        NbulletFill.fillAmount = (float)bulletInfo.remainNBullet / (float)bulletInfo.maxNBullet;

    }

    private void Start()
    {
        cam = FindObjectOfType<CinemachineVirtualCamera>();

        //shoot = FindObjectOfType<PlayerAttack>();
        muzzle = transform.Find("Muzzle").GetComponentInChildren<Transform>();
        BlackSmith = GameObject.Find("BlackSmithCanvas");
        fireSound = GetComponent<AudioSource>();
        reloadTime = new WaitForSeconds(2f);
        state = State.READY;        

    }
    // Update is called once per frame

    private void OnEnable()
    {
        NBullet.SetActive(true);
        NBulletBack.SetActive(true);
        //BulletText.text = magAmmo + " / " + weaponAmmo;
        NbulletFill.fillAmount = (float)bulletInfo.remainNBullet / (float)bulletInfo.maxNBullet;
    }

    private void OnDisable()
    {
        NBullet.SetActive(false);
        NBulletBack.SetActive(false);

        state = State.READY;
    }

    void Update()
    {
        NbulletFill.fillAmount = (float)bulletInfo.remainNBullet / (float)bulletInfo.maxNBullet;
        BulletText.text = magAmmo + " / " + bulletInfo.remainNBullet;

        if (BlackSmith != null && BlackSmith.activeSelf)
        {
            return;
        }

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
                fireSound.clip = EmptyMagAmmo;
                fireSound.Play();
            }
            // 장전키를 눌렀을 시
            else if (Input.GetKeyDown(KeyCode.R) && state != State.RELOADING)
            {
                StartCoroutine(ReLoading());
            }

        }

        // 탄창 용량 상관없이 장전을 누를때
        if(Input.GetKeyDown(KeyCode.R) && state != State.RELOADING)
        {
            StartCoroutine(ReLoading());
        }

        if(state == State.READY)
        {
            if(magAmmo <= 0)
            {
                magAmmo = 0;
                muzzlFlash.Stop();
                state = State.EMPTY;
            }

            if (Input.GetMouseButton(0) == true)
            {
                attackTimer += Time.deltaTime;

                if(attackTimer > attackSpeed)
                {
                    GameObject obj = null;
                    Rigidbody objRigid = null;
                    Bullet001 objDamage = null;

                    obj = PhotonPoolManager.P_instance.GetPoolObj(P_PoolObjType.BULLET);

                    if (obj != null)
                    {
                        obj.transform.position = muzzle.transform.position;
                        obj.transform.rotation = muzzle.transform.rotation;

                        objRigid = obj.GetComponent<Rigidbody>();
                        objDamage = obj.GetComponent<Bullet001>();

                        obj.gameObject.SetActive(true);

                        objDamage.riflebulletDamage = UpgradeManager.up_Instance.rifleDamage;
                        objRigid.velocity = cam.transform.forward * bulletSpeed;

                    }

                    magAmmo -= 1;

                    muzzlFlash.Play();
                    fireSound.clip = basicShot;
                    fireSound.volume = 0.4f;
                    fireSound.Play();

                    attackTimer = 0f;
                }
            }
            else
            {
                muzzlFlash.Stop();
            }
            // } 기본 사격 : 좌클릭

            if(Input.GetMouseButtonDown(1))
            {                
                UsingSkill(P_PoolObjType.BULLET);                              
            }

        }

    }

    private void UsingSkill(P_PoolObjType type)
    {
        useSkiil = true;
        // 남아있는 탄창이 스킬용량보다 작을 때 
        if(magAmmo < skillAmmo )
        {
            // 나가는 스킬 총알 갯수는 현재 탄창의 남은 양만큼
            for (int i = 0; i < magAmmo; i++)
            {
                Vector3 foward = cam.transform.forward;
                foward.x += Random.Range(xMax, xMin);
                foward.y += Random.Range(yMax, yMin);
                foward.z += Random.Range(xMax, xMin);

                GameObject obj = null;
                Rigidbody objRigid = null;
                Bullet001 objDamage = null;

                obj = PhotonPoolManager.P_instance.GetPoolObj(P_PoolObjType.BULLET);

                if(obj != null)
                {
                    obj.transform.position = muzzle.transform.position;
                    obj.transform.rotation = muzzle.transform.rotation;

                    objRigid = obj.GetComponent<Rigidbody>();
                    objDamage = obj.GetComponent<Bullet001>();

                    obj.gameObject.SetActive(true);

                    objDamage.riflebulletDamage = UpgradeManager.up_Instance.rifleDamage;
                    objRigid.velocity = foward * bulletSpeed;

                }

            }
            magAmmo -= magAmmo;
        }
        else
        {
            for (int i = 0; i < skillAmmo; i++)
            {
                Vector3 foward = cam.transform.forward;
                foward.x += Random.Range(xMax, xMin);
                foward.y += Random.Range(yMax, yMin);
                foward.z += Random.Range(xMax, xMin);

                GameObject obj = null;
                Rigidbody objRigid = null;
                Bullet001 objDamage = null;

                obj = PhotonPoolManager.P_instance.GetPoolObj(P_PoolObjType.BULLET);

                if (obj != null)
                {
                    obj.transform.position = muzzle.transform.position;
                    obj.transform.rotation = muzzle.transform.rotation;

                    objRigid = obj.GetComponent<Rigidbody>();
                    objDamage = obj.GetComponent <Bullet001>();

                    obj.gameObject.SetActive(true);

                    objDamage.riflebulletDamage = UpgradeManager.up_Instance.rifleDamage;
                    objRigid.velocity = foward * bulletSpeed;

                }

                magAmmo -= 1;
            }

        }

        muzzlFlash.Play();
        fireSound.clip = skillShot;
        fireSound.volume = 0.8f;
        fireSound.Play();

        useSkiil = false;
    }

    IEnumerator ReLoading()
    {
        state = State.RELOADING;

        int reloadBullet = 0;

        reloadBullet = magCapacity - magAmmo;

        fireSound.clip = CrimsonFirescale_Reload;
        fireSound.Play();

        if (reloadBullet > bulletInfo.remainNBullet)
        {
            magAmmo += bulletInfo.remainNBullet;
            bulletInfo.remainNBullet = 0;

            yield return reloadTime;

            // 재장전 시간 이후 공격준비 상태로 바꾸며 코루틴 종료
            state = State.READY;

            yield break;
        }
        // 재장전 시간
        yield return reloadTime;

        bulletInfo.remainNBullet -= reloadBullet;
        magAmmo += reloadBullet;

        // 재장전 시간 이후 공격준비 상태로 바꾸며 코루틴 종료
        state = State.READY;

        yield break;
    }
}
