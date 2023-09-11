using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rifle : MonoBehaviour
{
    // 총기의 현재 상태를 정의할 Enum : 발사가능, 탄창비어있음, 재장전 중
    public enum State { READY, EMPTY, Reloading}
    public State state {  get; private set; }

    // 사격시 생성할 총알 프리팹
    public GameObject bulletPrefab;
    // 총알이 생성될 총구 위치
    private Transform muzzle;

    // 사격시 총구 화염 파티클
    public ParticleSystem muzzlFlash;

    private AudioSource fireSound;
    public AudioClip basicShot;
    public AudioClip skillShot;
    public AudioClip EmptyMagAmmo;
    public AudioClip CrimsonFirescale_Reload;

    // 사격 간격시간
    public float attackSpeed = 0.1f;    
    private float attackTimer = 0f;    

    // 전체 최대 총알 수
    public int maxAmmoRemain = 90;
    // 남아있는 전체 총알 수
    public int ammoRemain;

    // 탄창 최대 용량
    public int magCapacity = 30;
    // 탄창 현재 총알 수
    public int magAmmo;

    public int skillAmmo = 10;

    public TMP_Text MagAmmoText;
    public TMP_Text AmmoRemainText;

    PlayerAttack shoot;

    private WaitForSeconds reloadTime;

    private void Start()
    {
        shoot = FindObjectOfType<PlayerAttack>();
        muzzle = transform.Find("Muzzle").GetComponentInChildren<Transform>();
        fireSound = GetComponent<AudioSource>();
        reloadTime = new WaitForSeconds(1.1f);

        // { 갖고있는 전체 총알, 현재 탄창 총알 텍스트 띄우기
        ammoRemain = maxAmmoRemain;
        AmmoRemainText.text = "" + maxAmmoRemain;
        magAmmo = magCapacity;
        MagAmmoText.text = "" + magCapacity;
        // } 갖고있는 전체 총알, 현재 탄창 총알 텍스트 띄우기

        state = State.READY;        

    }
    // Update is called once per frame
    void Update()
    {        
        if(transform.parent == null)
        {
            return;
        }

        // 탄창이 비어있는 상태라면
        if(state == State.EMPTY)
        {
            // 마우스 입력시 빈 탄창 소리 내기
            if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                fireSound.clip = EmptyMagAmmo;
                fireSound.Play();
            }
            // 장전키를 눌렀을 시
            else if (Input.GetKeyDown(KeyCode.R) && state != State.Reloading)
            {
                StartCoroutine(ReLoading());
            }

        }

        // 탄창 용량 상관없이 장전을 누를때
        if(Input.GetKeyDown(KeyCode.R) && state != State.Reloading)
        {
            StartCoroutine(ReLoading());
        }

        if(state == State.READY)
        {
            if(magAmmo <= 0)
            {
                magAmmo = 0;
                state = State.EMPTY;
            }

            if (shoot.isShoot == true)
            {
                attackTimer += Time.deltaTime;

                if(attackTimer > attackSpeed)
                {
                    Instantiate(bulletPrefab, muzzle.transform.position, muzzle.transform.rotation);
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

            if(shoot.useSkill == true)
            {                
                UsingSkill();                              
            }

        }

        AmmoRemainText.text = "" + ammoRemain;
        MagAmmoText.text = "" + magAmmo;

    }

    private void UsingSkill()
    {
        // 남아있는 탄창이 스킬용량보다 작을 때 
        if(magAmmo < skillAmmo )
        {
            // 나가는 스킬 총알 갯수는 현재 탄창의 남은 양만큼
            for (int i = 0; i < magAmmo; i++)
            {
                Instantiate(bulletPrefab, muzzle.transform.position, muzzle.transform.rotation);
            }
            magAmmo -= magAmmo;
        }
        else
        {
            for (int i = 0; i < skillAmmo; i++)
            {
                Instantiate(bulletPrefab, muzzle.transform.position, muzzle.transform.rotation);
                magAmmo -= 1;
            }

        }

        muzzlFlash.Play();
        fireSound.clip = skillShot;
        fireSound.volume = 0.8f;
        fireSound.Play();
    }

    IEnumerator ReLoading()
    {
        state = State.Reloading;

        int reloadBullet = 0;

        reloadBullet = magCapacity - magAmmo;

        fireSound.clip = CrimsonFirescale_Reload;
        fireSound.Play();

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
}
