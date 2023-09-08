using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool isShoot = false;
    public bool useSkill = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isShoot = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isShoot = false;
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
