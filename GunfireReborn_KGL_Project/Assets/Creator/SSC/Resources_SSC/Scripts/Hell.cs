using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hell : MonoBehaviour
{
    // 총기의 현재 상태를 정의할 Enum : 발사가능, 탄창비어있음, 재장전, 펌프액션
    public enum State { READY, EMPTY, RELOADING, PUMP_ACTION }
    public State state {  get; private set; }

    // 사격시 생성할 총알 프리팹
    public GameObject bulletPrefab;
    // 총알이 생성될 총구 위치
    private Transform muzzle;

    // 사격시 총구 화염 파티클
    public ParticleSystem muzzlFlash;

    private AudioSource fireSound;
    public AudioClip Hell_Shot;
    public AudioClip Hell_Reload;
    public AudioClip EmptyMagAmmo;

    // 사격 간격시간
    public float attackSpeed = 1f;
    public float skillSpeed = 0.5f;

    // 전체 최대 총알 수
    public int maxAmmoRemain = 24;

    // 남아있는 전체 총알 수
    public int ammoRemain;

    // 탄창 최대 용량
    public int magCapacity = 8;
    // 탄창 현재 총알 수
    public int magAmmo;

    public TMP_Text MagAmmoText;
    public TMP_Text AmmoRemainText;

    private WaitForSeconds reloadingTime;

    IEnumerator reload;

    private void Start()
    {        
        muzzle = transform.Find("Muzzle").GetComponentInChildren<Transform>();
        fireSound = GetComponent<AudioSource>();
        reloadingTime = new WaitForSeconds(1.0f);

        // { 갖고있는 전체 총알, 현재 탄창 총알 텍스트 띄우기
        ammoRemain = maxAmmoRemain;
        AmmoRemainText.text = "" + maxAmmoRemain;
        magAmmo = magCapacity;
        MagAmmoText.text = "" + magCapacity;
        // } 갖고있는 전체 총알, 현재 탄창 총알 텍스트 띄우기

        // 장전 코루틴 담아두기
        reload = ReLoading();
        // 총의 시작은 레디상태
        state = State.READY;       

    }
    // Update is called once per frame
    void Update()
    {        
        // 플레이어의 손에 있는것이 아니라면 동작하지 않는다.
        if(transform.parent == null)
        {
            return;
        }

        // 펌프액션 (공격 중 상태) 상태가 된다면 장전을 멈춘다.
        if(state == State.PUMP_ACTION)
        {
            StopCoroutine(reload);
        }

        // 탄창이 비어있는 상태라면
        if(state == State.EMPTY)
        {
            // 마우스 입력시 빈 탄창 소리 내기
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                fireSound.clip = EmptyMagAmmo;
                fireSound.Play();
            }            
            // 장전키를 눌렀을 시
            else if (Input.GetKeyDown(KeyCode.R) && state != State.RELOADING)
            {
                state = State.RELOADING;
                StartCoroutine(reload);
            }

            // 그 외에 상황에는 밑으로 진행하지 않는다 ( 공격 불가 )
            return;
        }

        // 평상시 장전
        if(Input.GetKeyDown(KeyCode.R) && state != State.RELOADING)
        {
            state = State.RELOADING;
            StartCoroutine(reload);
        }

        // 레디 상태에서만 사격 가능
        if(state == State.READY)
        {
            // 현재 탄창총알이 0보다 작아진다면
            if(magAmmo <= 0)
            {
                // 음수로 넘어가기 방지
                magAmmo = 0;
                // 탄창이 비어있는 상태로 변경
                state = State.EMPTY;
            }

            // PlayerAttack Cs파일에서 마우스클릭을 통해 bool 값 신호를 받음
            if (Input.GetMouseButtonDown(0))
            {
                state = State.PUMP_ACTION;
                StartCoroutine(Attack());
            }
            // } 기본 사격 : 좌클릭
        }

        AmmoRemainText.text = "" + ammoRemain;
        MagAmmoText.text = "" + magAmmo;

    }

    IEnumerator Attack()
    {       
        for (int i = 0; i < 10; i++)
        {
            Instantiate(bulletPrefab, muzzle.transform.position, muzzle.transform.rotation);
        }
        magAmmo -= 1;
      
        fireSound.clip = Hell_Shot;
        fireSound.Play();

        yield return reloadingTime;

        state = State.READY;

        yield break;
    }

    IEnumerator ReLoading()
    {
        fireSound.clip = Hell_Reload;

        // 현재 총알이 한 탄창의 총량보다(8발) 적은동안 반복
        while (magAmmo < magCapacity)
        {
            // 남아있는 총알 수가 0 이하가 될시
            if (ammoRemain <= 0)
            {
                ammoRemain = 0;
                yield break;
            }

            fireSound.Play();

            yield return reloadingTime;
            ammoRemain -= 1;
            magAmmo += 1;

            // 한발 장전이 끝날때마다 공격 가능 상태 ( 펌프액션 샷건 )
            state = State.READY;

        }

        yield break;

    }
}
