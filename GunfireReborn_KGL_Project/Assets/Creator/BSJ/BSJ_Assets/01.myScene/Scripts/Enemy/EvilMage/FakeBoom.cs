using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FakeBoom : MonoBehaviour
{
    public float scaleFactor = 1f;          // 최종 스케일
    public float duration = 0.8f;           // 변경에 걸릴 시간
    private Vector3 startScale;             // 초기 스케일
    private float startTime;                // 시작 시간

    private void Start()
    {
        startScale = transform.localScale;  // 초기 스케일 저장
        startTime = Time.time;              // 시작 시간 저장
    }

    private void Update()
    {
        // 현재 시간에서 시작 시간을 빼서 진행된 시간 계산
        float elapsed = Time.time - startTime;

        // 스케일을 천천히 변경하는 비율 계산 (0에서 1까지)
        float t = Mathf.Clamp01(elapsed / duration);

        // 스케일 값을 초기 스케일에서 최종 스케일로 보간하여 변경
        transform.localScale = Vector3.Lerp(startScale, Vector3.one * scaleFactor, t);

        // 변경이 완료 되면 스케일 변경 중단
        if(t >= 1.0f)
        {
            transform.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        startTime = Time.time;
    }


    // 초기 스케일로 변경
    private void OnDisable()
    {
        transform.localScale = startScale;
    }
}
