using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FloorFireMovement : MonoBehaviour
{
    public Vector3 targetPos;       // 이동할 목표 위치
    public float duration = 5.0f;   // 이동에 걸릴 시간

    Vector3 initialPosition;    // 이동 시작 위치
    float startTime;            // 이동 시작 시간

    bool isMoving = false;      // 이동 중인지 체크


    private void Awake()
    {
        // 현재 위치를 시작 위치로 셋팅
        initialPosition = transform.position;
    }

    private void OnEnable()
    {
        // 위치를 다시 셋팅
        transform.position = initialPosition;
    }

    void Update()
    {
        if(!isMoving)
        {
            StartCoroutine(MoveOverTime());
        }
    }

    IEnumerator MoveOverTime()
    {
        isMoving = true;
        startTime = Time.time;

        // 보간을 사용하여 게임 오브젝트를 부드럽게 이동시키는데 사용
        while(Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            // 시작 위치에서 목표 위치까지 보간해서 이동
            transform.position = Vector3.Lerp(initialPosition, targetPos, t);

            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        gameObject.SetActive(false);
    }
}
