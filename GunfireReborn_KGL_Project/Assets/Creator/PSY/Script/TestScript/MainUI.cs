using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    #region Dash 
    private Image t_DashBG;
    private TextMeshProUGUI text_dashCoolTime;
    #endregion

    #region DashEffect
    private Transform dashEffectGroup;
    private Transform[] dashEffectImages;
    #endregion

    private void Start()
    {
        #region Dash
        t_DashBG = transform.Find("T_DashBG").GetComponent<Image>();
        text_dashCoolTime = t_DashBG.transform.Find("Text_dashCoolTime").GetComponent<TextMeshProUGUI>();

        t_DashBG.enabled = false;           // Dash UI BG 비활성화
        text_dashCoolTime.enabled = false;  // Dash UI Text 비활성화
        #endregion

        #region DashEffect

        dashEffectGroup = transform.Find("DashEffectGroup").GetComponent<Transform>();

        // dashEffectGroup 자식 오브젝트 갯수만큼의 dashEffectImages 배열 생성
        dashEffectImages = new Transform[dashEffectGroup.childCount];  

        for ( int i = 0; i < dashEffectGroup.childCount; i++ )
        {
            // dashEffectImages에 dashEffectGroup의 자식을 넣는다.
            dashEffectImages[i] = dashEffectGroup.GetChild(i);
        }
        #endregion
    }

    #region Dash
    /// <summary>
    /// Dash 쿨타임 코루틴 실행 함수
    /// </summary>
    public void CountDashCoolTime()
    {
        StartCoroutine(DecreaseDash(3f));
    }

    /// <summary>
    /// Dash 쿨타임 감소 코루틴 함수
    /// </summary>
    public IEnumerator DecreaseDash(float cool)
    {
        t_DashBG.enabled = true;          // Dash UI BG 활성화
        text_dashCoolTime.enabled = true; // Dash UI Text 활성화

        float coolText = 3;  // 쿨타임 Text의 초기 값을 3으로 설정

        while (t_DashBG.fillAmount < 1)  // Dasg UI BG의 Fill이 1이 될 때까지 반복
        {
            t_DashBG.fillAmount += 1 * Time.smoothDeltaTime / cool;  // 쿨타임을 기준으로 fill을 프레임마다 더해준다.
            coolText -= Time.smoothDeltaTime;  // 쿨타임 Text를 프레임마다 빼준다.
            text_dashCoolTime.text = string.Format("{0:N1}", coolText);  // 쿨타임 Text를 소수점 한자리까지 나오게 출력한다.

            yield return null;
        }

        t_DashBG.fillAmount = 0f;          // Dash UI BG의 fill을 0으로 설정한다.
        t_DashBG.enabled = false;          // Dash UI BG 비활성화
        text_dashCoolTime.enabled = false; // Dash UI Text 비활성화
    }
    #endregion

    #region DashEffect
    /// <summary>
    /// Dash 발동 시 실행되는 코루틴 함수
    /// </summary>
    public IEnumerator DashEffect()
    {
        int dashEffectRand = Random.Range(0, dashEffectImages.Length);

        dashEffectImages[dashEffectRand].transform.localScale = Vector3.one;   // Dash Effect Image의 Scale을 1로 설정한다.

        yield return new WaitForSeconds(0.2f);  // 2초 딜레이를 준다.

        dashEffectImages[dashEffectRand].transform.localScale = Vector3.zero;  // Dash Effect Image의 Scale을 0으로 설정한다.
    }
    #endregion
}
