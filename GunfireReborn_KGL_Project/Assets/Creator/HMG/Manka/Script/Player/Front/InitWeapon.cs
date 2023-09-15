using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class InitWeapon : MonoBehaviourPun
{
    private WeaponManager player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<WeaponManager>();
        // Front_weapons 배열에 자식 게임 오브젝트들을 넣습니다.
        for (int i = 0; i < player.Front_weapons.Length; i++)
        {
            Transform child = transform.GetChild(2).GetChild(i); // 자식 게임 오브젝트 가져오기
            player.Front_weapons[i] = child.gameObject; // 배열에 넣기
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
