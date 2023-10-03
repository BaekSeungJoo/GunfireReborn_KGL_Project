using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public WeaponManager1 weapon;
    public Image rifle;
    public Image shotgun;
    public Image pistol;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SetWeaponManager", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(weapon == null)
        {
            return;
        }
        if (weapon.slotWeapons[weapon.CheckActiveslot()] == "CrimsonFirescale")
        {
            rifle.gameObject.SetActive(true);
            shotgun.gameObject.SetActive(false);
            pistol.gameObject.SetActive(false);
        }
        else if(weapon.slotWeapons[weapon.CheckActiveslot()] == "Shotgun")
        {
            rifle.gameObject.SetActive(false);
            shotgun.gameObject.SetActive(true);
            pistol.gameObject.SetActive(false);
        }
        else if(weapon.slotWeapons[weapon.CheckActiveslot()] == "Pistol")
        {
            rifle.gameObject.SetActive(false);
            shotgun.gameObject.SetActive(false);
            pistol.gameObject.SetActive(true);
        }
    }

    void SetWeaponManager()
    {
        weapon = FindObjectOfType<CinemachineVirtualCamera>().transform.parent.gameObject.GetComponent<WeaponManager1>();
    }
}
