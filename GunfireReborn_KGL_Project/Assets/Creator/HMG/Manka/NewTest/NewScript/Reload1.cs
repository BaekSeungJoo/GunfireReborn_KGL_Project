using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Reload1 : MonoBehaviour
{
    public GameObject playerGun;
    public Transform weaponPosition;
    private bool isReloading;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Reload") && !isReloading)
        {

            // 총을 천천히 아래로 내리는 코루틴을 시작합니다.
            //StartCoroutine(LowerGun());
            animator.SetTrigger("Reload");
        }
    }
    private void Search()
    {// 현재 활성화된 무기를 찾는 함수.
        // weaponPosition 하위의 모든 GameObject를 배열에 넣습니다.
        int childCount = weaponPosition.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if (weaponPosition.GetChild(i).gameObject.activeSelf == true)
            {
                playerGun = weaponPosition.GetChild(i).gameObject;
            }
        }
    }

    IEnumerator LowerGun()
    {
        // 재장전 중인 상태로 설정합니다.
        isReloading = true;
        Search();
        // 팔과 총을 천천히 아래로 내리는 처리를 구현합니다.
        float elapsedTime = 0f;
        float duration = 0.5f; // 내리는 시간
        Vector3 initialArmLocalPosition = playerGun.transform.localPosition;
        Vector3 targetArmLocalPosition = initialArmLocalPosition - playerGun.transform.up * 0.5f; // 아래로 내리기

        Quaternion initialGunLocalRotation = playerGun.transform.localRotation;
        Quaternion targetGunLocalRotation = Quaternion.Euler(0f, -90f, -45f); // Z 축만 회전

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            playerGun.transform.localPosition = Vector3.Lerp(initialArmLocalPosition, targetArmLocalPosition, t);
            playerGun.transform.localRotation = Quaternion.Slerp(initialGunLocalRotation, targetGunLocalRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 대기합니다. (예: 총을 아래로 내린 후 3초 동안 대기)
        yield return new WaitForSeconds(1.0f);

        // 팔과 총을 원래 위치로 돌리는 처리를 구현합니다.
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            playerGun.transform.localPosition = Vector3.Lerp(targetArmLocalPosition, initialArmLocalPosition, t);
            playerGun.transform.localRotation = Quaternion.Slerp(targetGunLocalRotation, initialGunLocalRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 재장전 완료 후 상태를 원래대로 되돌립니다.
        isReloading = false;
    }

}