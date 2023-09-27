using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager up_Instance;

    public int rifleUp = 0;
    public int shotgunUp = 0;


    public int pistolDamage = 5;
    public int rifleDamage = 4;
    public int shotgunDamage = 3;

    private void Awake()
    {
        if(up_Instance == null )
        {
            up_Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void RifleUp()
    {
        rifleUp++;
        rifleDamage = rifleDamage + (rifleUp * 4);
    }

    public void ShotgunUp()
    {
        shotgunUp++;
        shotgunDamage = shotgunDamage + (shotgunUp * 3);
    }
}
