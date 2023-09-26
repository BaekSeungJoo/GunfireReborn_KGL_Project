using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class BlackSmithUI : MonoBehaviour
{
    private GameObject player;                      // 플레이어
    private WeaponManager1 weapon;                  // 웨폰매니저

    public TextMeshProUGUI weaponName1;             // 슬롯1번 이름
    public TextMeshProUGUI weaponName2;             // 슬롯2번 이름
    
    public TextMeshProUGUI weaponDamage1;           // 슬롯1번 데미지
    public TextMeshProUGUI weaponDamage2;           // 슬롯2번 데미지
   
    public TextMeshProUGUI weaponAmmoCount1;        // 슬롯1번 탄창
    public TextMeshProUGUI weaponAmmoCount2;        // 슬롯2번 탄창

    public TextMeshProUGUI Gold;                    // 골드

    public Image weaponImage1;                      // 1슬롯불꽃비늘웨폰이미지
    public Image weaponImage2;                      // 1술롯지옥웨폰이미지

    public Image weaponImage3;                      // 2슬롯불꽃비늘웨폰이미지
    public Image weaponImage4;                      // 2술롯지옥웨폰이미지

    public Image bulletImage1;                      // 1슬롯 특수탄이미지
    public Image bulletImage2;                      // 1슬롯 대형탄이미지

    public Image bulletImage3;                      // 2슬롯 특수탄이미지
    public Image bulletImage4;                      // 2슬롯 대형탄이미지

    public TextMeshProUGUI bulletText1;             // 1슬롯 총알 Text
    public TextMeshProUGUI bulletText2;             // 2슬롯 총알 Text

    string slot1Weapon;                             // 플레이어의 weapon1이름을 담을 변수
    string slot2Weapon;                             // 플레이어의 weapon2이름을 담을 변수

    public TextMeshProUGUI weaponInfo1;             // 슬롯1웨폰설명
    public TextMeshProUGUI weaponInfo2;             // 슬롯2웨폰설명

    public TextMeshProUGUI inforceCountText;        // 강화횟수텍스트
    private int inforceCount = 3;                   // 강화횟수

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CinemachineVirtualCamera>().transform.parent.gameObject;
        weapon = player.GetComponent<WeaponManager1>();
        inforceCountText.text = "잔여 강화 횟수 : 3";

    }

    private void OnEnable()
    {
        UpdateWeapon();
        UpdateText();
    }
    // Update is called once per frame
    void Update()
    {
        Gold.text = (player.GetComponent<PlayerGold>().Gold).ToString();
        if (slot1Weapon == "CrimsonFirescale")
        {
            weaponDamage1.text = "기초 대미지 "  + (UpgradeManager.up_Instance.rifleDamage).ToString();
        }
        else if (slot1Weapon == "Shotgun")
        {
            weaponDamage1.text = "기초 대미지 " + (UpgradeManager.up_Instance.shotgunDamage).ToString();
        }

        if (slot2Weapon == "CrimsonFirescale")
        {
            weaponDamage2.text = "기초 대미지 " + (UpgradeManager.up_Instance.rifleDamage).ToString();
        }
        else if (slot2Weapon == "Shotgun")
        {
            weaponDamage2.text = "기초 대미지 " + (UpgradeManager.up_Instance.shotgunDamage).ToString();
        }
    }

    #region 업데이트 웨폰
    private void UpdateWeapon()
    {
        if (weapon.slotWeapons[0] != null)
        {
            slot1Weapon = weapon.slotWeapons[0];
        }
        if (weapon.slotWeapons[1] != null)
        {
            slot2Weapon = weapon.slotWeapons[1];
        }
    }
    #endregion
    private void UpdateText()
    {
        //골드텍스트 업데이트
        Gold.text = (player.GetComponent<PlayerGold>().Gold).ToString();
        //슬롯1웨폰무기이름, 텍스트 업데이트
        if (slot1Weapon == "CrimsonFirescale")
        {
            weaponName1.text = "불꽃 비늘";
            weaponAmmoCount1.text = "탄창용량 30";
            bulletText1.text = "특수탄";
            weaponInfo1.text = "◆ 라이플입니다.";
            weaponDamage1.text = "기초 대미지 " + (UpgradeManager.up_Instance.rifleDamage).ToString();
        }
        else if (slot1Weapon == "Shotgun")
        {
            weaponName1.text = "지 옥";
            weaponAmmoCount1.text = "탄창용량 8";
            bulletText1.text = "대형탄";
            weaponInfo1.text = "◆ 샷건입니다.";
            weaponDamage1.text = "기초 대미지 " + (UpgradeManager.up_Instance.shotgunDamage).ToString();

        }
        //슬롯2웨폰무기이름텍스트 업데이트
        if (slot2Weapon == "CrimsonFirescale")
        {
            weaponName2.text = "불꽃 비늘";
            weaponAmmoCount2.text = "탄창용량 30";
            bulletText2.text = "특수탄";
            weaponInfo2.text = "◆ 라이플입니다.";
            weaponDamage2.text = "기초 대미지  " + (UpgradeManager.up_Instance.rifleDamage).ToString();
        }
        else if (slot2Weapon == "Shotgun")
        {
            weaponName2.text = "지 옥";
            weaponAmmoCount2.text = "탄창용량 8";
            bulletText2.text = "대형탄";
            weaponInfo2.text = "◆ 샷건입니다.";
            weaponDamage2.text = "기초 대미지 " + (UpgradeManager.up_Instance.shotgunDamage).ToString();
        }
    }

    public void UpGrade1()
    {
        if (inforceCount >= 1 && player.GetComponent<PlayerGold>().Gold >= 100)
        {
            if (slot1Weapon == "CrimsonFirescale")
            {
                UpgradeManager.up_Instance.RifleUp();
                inforceCount -= 1;
                inforceCountText.text = "잔여 강화 횟수 : " + inforceCount.ToString();
                player.GetComponent<PlayerGold>().Gold -= 100;
            }
            else if (slot1Weapon == "Shotgun")
            {
                UpgradeManager.up_Instance.ShotgunUp();
                inforceCount -= 1;
                inforceCountText.text = "잔여 강화 횟수 : " + inforceCount.ToString();
                player.GetComponent<PlayerGold>().Gold -= 100;
            }
        }
    }

    public void UpGrade2()
    {
        if (inforceCount >= 1 && player.GetComponent<PlayerGold>().Gold>=100)
        {
            if (slot2Weapon == "CrimsonFirescale")
            {
                UpgradeManager.up_Instance.RifleUp();
                inforceCount -= 1;
                inforceCountText.text = "잔여 강화 횟수 : " + inforceCount.ToString();
                player.GetComponent<PlayerGold>().Gold -= 100;
            }
            else if (slot2Weapon == "Shotgun")
            {
                UpgradeManager.up_Instance.ShotgunUp();
                inforceCount -= 1;
                inforceCountText.text = "잔여 강화 횟수 : " + inforceCount.ToString();
                player.GetComponent<PlayerGold>().Gold -= 100;
            }
        }
    }
}
