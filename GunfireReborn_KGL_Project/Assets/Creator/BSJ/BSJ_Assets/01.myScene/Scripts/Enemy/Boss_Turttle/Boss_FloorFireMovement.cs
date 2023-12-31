using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FloorFireMovement : MonoBehaviour
{
    public GameObject startPos;        // 이동 시작 위치

    private float moveSpeed = 30f;     // 이동 속도
    private float lifeTime = 2f;      // 이펙트 활성화 시간
    private float timer = 0f;         // 비활성화까지 더해질 시간

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * moveSpeed;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > lifeTime)
        {
            gameObject.SetActive(false);
            timer = 0f;
        }
    }
    private void OnDisable()
    {
        // 위치를 다시 셋팅
        gameObject.transform.localPosition = Vector3.zero;
    }
}
