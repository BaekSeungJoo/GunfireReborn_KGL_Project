
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<<< HEAD:GunfireReborn_KGL_Project/Assets/Creator/HMG/Manka/NewTest/NewScript/PlayerAttack.cs
public class PlayerAttack : MonoBehaviour
========
public class PlayerAttack1 : MonoBehaviour
>>>>>>>> origin/SSC:GunfireReborn_KGL_Project/Assets/Creator/HMG/Manka/NewTest/NewScript/PlayerAttack1.cs
{
    public bool isShoot = false;
    public bool useSkill = false;
    public bool isShootPistol = false;

    // Update is called once per frame
    void Update()
    {
<<<<<<<< HEAD:GunfireReborn_KGL_Project/Assets/Creator/HMG/Manka/NewTest/NewScript/PlayerAttack.cs

========
>>>>>>>> origin/SSC:GunfireReborn_KGL_Project/Assets/Creator/HMG/Manka/NewTest/NewScript/PlayerAttack1.cs
        //클릭시 필드에있는 다른무기에서 발사가되는문제 , 필드와 플레이어가 가지고있는 무기가 다르니 상관은없음.
        if (Input.GetMouseButton(0))
        {
            isShoot = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isShoot = false;
            //isShootPistol = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isShootPistol = true;
        }
        // } 마우스 좌클릭


        // { 마우스 우클릭
        if (Input.GetMouseButtonDown(1))
        {
            useSkill = true;
        }
        else
        {
            useSkill = false;
        }
    }
}
