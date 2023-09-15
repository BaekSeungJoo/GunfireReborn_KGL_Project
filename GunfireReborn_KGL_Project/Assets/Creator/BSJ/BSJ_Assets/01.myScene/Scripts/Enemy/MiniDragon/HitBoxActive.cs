using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HitBoxActive : MonoBehaviour
{
    // 오크 Enemy도 동일한 스크립트 사용함.

    public GameObject HitBox;           // 공격 범위를 가진 오브젝트

    private float ticDamage = 0.3f;     // 0.3초마다 데미지를 입음
    private float timer = 0f;           // 데미지 타이머

    public void Update()
    {
        timer += Time.deltaTime;

        // 틱 공격 시작
        if (timer < ticDamage)
        {
            HitBox.SetActive(true);
        }

        if (timer > ticDamage)
        {
            HitBox.SetActive(false);
            timer = 0f;
        }
    }
}
