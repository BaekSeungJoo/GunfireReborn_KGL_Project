using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    // 총기의 현재 상태를 정의할 Enum : 발사가능, 탄창비어있음, 재장전
    public enum State { Ready, Empty, Reloading}
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

    // 사격 간격시간
    public float attackSpeed = 0.01f;
    private float attackTimer = 0f;

    private bool useskill = false;

    PlayerController shoot;

    private void Start()
    {
        shoot = FindObjectOfType<PlayerController>();
        muzzle = transform.Find("Muzzle").GetComponentInChildren<Transform>();
        fireSound = GetComponent<AudioSource>();
        //muzzlFlash = GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        // { 기본 사격 : 좌클릭
        if(shoot.isShoot == true)
        {
            attackTimer += Time.deltaTime;

            if(attackTimer > attackSpeed)
            {
                
                Instantiate(bulletPrefab, muzzle.transform.position, transform.rotation);         
                muzzlFlash.Play();
                fireSound.clip = basicShot;
                fireSound.volume = 0.4f;
                fireSound.Play();

                shoot.camRotate.x += 50f;
                attackTimer = 0f;
            }
        }
        else
        {
            muzzlFlash.Stop();
        }
        // } 기본 사격 : 좌클릭


        // { 스킬 사격 : 우클릭
        if(useskill == false)
        {
            UsingSkill();
        }
        // } 스킬 사격 : 우클릭


    }

    private void UsingSkill()
    {
        if (shoot.useSkill == true)
        {
            useskill = true;
            for (int i = 0; i < 10; i++)
            {
                Instantiate(bulletPrefab, muzzle.transform.position, transform.rotation);
            }

            muzzlFlash.Play();
            fireSound.clip = skillShot;
            fireSound.volume = 0.8f;
            fireSound.Play();

            useskill = false;
        }
    }
}
