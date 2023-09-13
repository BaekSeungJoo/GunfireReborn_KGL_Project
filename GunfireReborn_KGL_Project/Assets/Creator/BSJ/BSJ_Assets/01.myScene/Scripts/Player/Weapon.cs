using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;

    // public Camera camera;
    public GameObject bulletSpawnPoint;

    public float fireRate;

    // [Header("VFX")]
    public GameObject bullet;
    // public GameObject hitVFX;

    private float nextFire;

    [Header("Ammo")]
    public int mag = 5;
    public int ammo = 30;
    public int magAmmo = 30;

    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    [Header("Animation")]
    public Animation anim;
    public AnimationClip reload;

    [Header("Recoil Settings")]
    //[Range(0, 1)]
    //public float recoilPercent = 0.3f;

    [Range(0, 2)]
    public float recoverPercent = 0.7f;

    [Space]
    public float recoilUp = 1f;
    public float recoilBack = 0f;

    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    private float recoilLength;
    private float recoverLength;

    private bool recoiling;
    public bool recovering;

    private void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;

        originalPosition = transform.localPosition;

        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;
    }

    private void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
        
        if (Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 && anim.isPlaying == false)
        {
            nextFire = 1 / fireRate;
            ammo--;

            magText.text = mag.ToString();
            ammoText.text = ammo + " / " + magAmmo;

            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && mag > 0 && anim.isPlaying == false)
        {
            ReLoad();
        }

        if(recoiling)
        {
            Recoil();
        }

        if(recovering)
        {
            Recovering();
        }
    }

    void ReLoad()
    {
        anim.Play(reload.name);

        if (mag > 0)
        {
            mag--;

            ammo = magAmmo;
        }

        magText.text = mag.ToString();
        ammoText.text = ammo + " / " + magAmmo;
    }

    void Fire()
    {
        recoiling = true;
        recovering = false;

        PhotonNetwork.Instantiate(bullet.name, bulletSpawnPoint.transform.position, transform.rotation);
        
        //Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        //RaycastHit hit;

        //if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        //{
        //    // 맞은 지점에서 이펙트 생성
        //    PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);

        //    // 플레이어 타격
        //    if (hit.transform.gameObject.GetComponent<Health>())
        //    {
        //        // 무기 데미지에 의한 점수
        //        PhotonNetwork.LocalPlayer.AddScore(damage);

        //        // 킬에 의한 점수
        //        if (damage >= hit.transform.gameObject.GetComponent<Health>().health)
        //        {
        //            // kill
        //            RoomManager.instance.kills++;
        //            RoomManager.instance.SetHashes();

        //            PhotonNetwork.LocalPlayer.AddScore(100);
        //        }

        //        hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
        //    }

        //    // 적 타격
        //    if (hit.transform.gameObject.GetComponent<EnemyHealth>())
        //    {
        //        hit.transform.gameObject.GetComponent<PhotonView>().RPC("EnemyTakeDamage", RpcTarget.All, damage);
        //    }
        //}
    }

    void Recoil()
    {
        Vector3 finalPosition = new Vector3(originalPosition.x, originalPosition.y + recoilUp, originalPosition.z - recoilBack);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);

        if(transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }

    void Recovering()
    {
        Vector3 finalPosition = originalPosition;

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }
}
