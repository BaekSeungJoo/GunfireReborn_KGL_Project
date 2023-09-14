using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Resources;

public class Hell001 : MonoBehaviour
{
    // 총기의 현재 상태를 정의할 Enum : 발사가능, 탄창비어있음, 재장전, 펌프액션
    public enum State { READY, EMPTY, RELOADING, PUMP_ACTION }
    // 상태를 관찰할 프로퍼티 state
    public State state {  get; private set; }

    // 총알이 생성될 총구 위치
    private Transform muzzle;

    // 사격시 총구 화염 파티클
    public ParticleSystem muzzlFlash;

    // 각종 오디오 클립
    private AudioSource fireSound;
    public AudioClip Hell_Shot;
    public AudioClip Hell_Reload;
    public AudioClip EmptyMagAmmo;

    // 전체 최대 총알 수
    private int maxAmmoRemain = 24;

    // 남아있는 전체 총알 수
    private int ammoRemain;

    // 탄창 최대 용량
    private int magCapacity = 8;
    // 탄창 현재 총알 수
    private int magAmmo;

    //public TMP_Text MagAmmoText;
    //public TMP_Text AmmoRemainText;

    // 사격 간격시간
    private WaitForSeconds attackSpeed;
    private WaitForSeconds reloadingTime;

    // 장전중 사격시 장전 코루틴을 멈추기위한 Reloading() 코루틴을 담을 reload

    IEnumerator reload;

    private void Start()
    {
        muzzle = transform.Find("Muzzle").GetComponentInChildren<Transform>();
        fireSound = GetComponent<AudioSource>();
        reloadingTime = new WaitForSeconds(1.0f);
        attackSpeed = new WaitForSeconds(0.75f);

        // { 갖고있는 전체 총알, 현재 탄창 총알 텍스트 띄우기
        ammoRemain = maxAmmoRemain;
        //AmmoRemainText.text = "" + maxAmmoRemain;
        magAmmo = magCapacity;
        //MagAmmoText.text = "" + magCapacity;
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
        //if(transform.parent == null)
        //{
        //    return;
        //}

        // 탄창이 비어있는 상태라면
        if (state == State.EMPTY)
        {
            // 마우스 입력시 빈 탄창 소리 내기
            if (Input.GetMouseButtonDown(0))
            {
                fireSound.clip = EmptyMagAmmo;
                fireSound.Play();
            }
            // 장전키를 눌렀을 시
            else if (Input.GetKeyDown(KeyCode.R) && state != State.RELOADING)
            {
                state = State.RELOADING;
                fireSound.clip = Hell_Reload;
                StartCoroutine(reload);
            }

            // 그 외에 상황에는 밑으로 진행하지 않는다 ( 공격 불가 )
            return;
        }

        // 평상시 장전
        if(Input.GetKeyDown(KeyCode.R) && state != State.RELOADING)
        {
            state = State.RELOADING;
            fireSound.clip = Hell_Reload;
            StartCoroutine(reload);
        }

        // 레디, 재장전 중 사격 가능
        if(state == State.READY || state == State.RELOADING)
        {
            if (Input.GetMouseButtonDown(0))
            {
                state = State.PUMP_ACTION;
                StartCoroutine(Attack());
            }
        }

        //AmmoRemainText.text = "" + ammoRemain;
        //MagAmmoText.text = "" + magAmmo;

    }

    IEnumerator Attack()
    {
        StopCoroutine(reload);
        reload = ReLoading();

        for (int i = 0; i < 10; i++)
        {
            GameObject obj = ObjectPoolManager.instance.GetPoolObj(PoolObjType.HELLBULLET);

            obj.transform.position = muzzle.transform.position;
            obj.transform.rotation = muzzle.transform.rotation;

            obj.gameObject.SetActive(true);
        }

        magAmmo -= 1;
        fireSound.clip = Hell_Shot;
        fireSound.Play();

        // 현재 탄창총알이 0보다 작아진다면
        if (magAmmo <= 0)
        {
            // 음수로 넘어가기 방지
            magAmmo = 0;
            // 탄창이 비어있는 상태로 변경
            state = State.EMPTY;

            yield return attackSpeed;
            yield break;
        }

        yield return attackSpeed;

        state = State.READY;
        yield break;
    }

    IEnumerator ReLoading()
    {
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
            magAmmo += 1;
            ammoRemain -= 1;

        }

    }

}
