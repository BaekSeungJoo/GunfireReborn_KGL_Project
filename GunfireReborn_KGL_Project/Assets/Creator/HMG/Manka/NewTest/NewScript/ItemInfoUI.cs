using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using Cinemachine;
using Photon.Realtime;

public class ItemInfoUI: MonoBehaviour
{
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponDamage;
    public TextMeshProUGUI weaponAmmo;
    public TextMeshProUGUI weaponType;
    public TextMeshProUGUI weaponInfo;

    public Image rifleImage;
    public Image shotgunImage;
    public Image rifleAmmoImage;
    public Image shotgunAmmoImage;

    private ItemGet1 itemRay;

    private void Start()
    {
        itemRay = FindObjectOfType<CinemachineVirtualCamera>().transform.parent.gameObject.GetComponent<ItemGet1>();
    }



    public void  SetItemInfo()
    {
        if(itemRay.hitInfo.collider.gameObject.name == "CrimsonFirescale(get)(Clone)")
        {
            weaponName.text = "불꽃비늘" +"(0강)";
            weaponDamage.text = "기초 대미지 " + "4";
            weaponAmmo.text = "탄창용량" + "30";
            weaponType.text = "일반탄";
            weaponInfo.text = "◆ 30발 라이플입니다.";
            rifleImage.gameObject.SetActive(true);
            rifleAmmoImage.gameObject.SetActive(true);
            shotgunImage.gameObject.SetActive(false);
            shotgunAmmoImage.gameObject.SetActive(false);

        }
        else if (itemRay.hitInfo.collider.gameObject.name == "Shotgun(get)(Clone)")
        {
            weaponName.text = "지 옥" + "(0강)";
            weaponDamage.text = "기초 대미지 " + "3";
            weaponAmmo.text = "탄창용량" + "8";
            weaponType.text = "대형탄";
            weaponInfo.text = "◆ 8발 샷건입니다.";
            shotgunImage.gameObject.SetActive(true);
            shotgunAmmoImage.gameObject.SetActive(true);
            rifleImage.gameObject.SetActive(false);
            rifleAmmoImage.gameObject.SetActive(false);
        }
        
    }
        
}
