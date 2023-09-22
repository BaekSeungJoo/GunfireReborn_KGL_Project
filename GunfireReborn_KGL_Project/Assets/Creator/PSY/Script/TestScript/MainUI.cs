using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    #region Dash 
    private Image t_DashBG;
    private TextMeshProUGUI text_dashCoolTime;
    #endregion

    private void Start()
    {
        #region Dash
        t_DashBG = transform.Find("T_DashBG").GetComponent<Image>();
        text_dashCoolTime = t_DashBG.transform.Find("Text_dashCoolTime").GetComponent<TextMeshProUGUI>();

        t_DashBG.enabled = false;
        text_dashCoolTime.enabled = false;
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
        t_DashBG.enabled = true;
        text_dashCoolTime.enabled = true;

        float coolText = 3;

        while (t_DashBG.fillAmount < 1)
        {
            t_DashBG.fillAmount += 1 * Time.smoothDeltaTime / cool;
            coolText -= Time.smoothDeltaTime;
            text_dashCoolTime.text = string.Format("{0:N1}", coolText);

            yield return null;
        }

        t_DashBG.fillAmount = 0f;
        t_DashBG.enabled = false;

        text_dashCoolTime.enabled = false;
    }
    #endregion
}
