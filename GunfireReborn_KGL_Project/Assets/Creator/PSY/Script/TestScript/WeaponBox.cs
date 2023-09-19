using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#region WeaponData 클래스
public class WeaponData
{
    public string name;
    public int damage;
    public int bulletSize;
    public float criticalPer;
    public string bulletType;
    public string info;
    public string use;

    /// <summary>
    /// 기본적인 생성자
    /// </summary>
    public WeaponData() { }

    /// <summary>
    /// 외부데이터를 세팅해주는 생성자
    /// </summary>
    public WeaponData(string name, int damage, int bulletSize, float criticalPer, string bulletType, string info, string use)
    {
        this.name = name;
        this.damage = damage;
        this.bulletSize = bulletSize;
        this.criticalPer = criticalPer;
        this.bulletType = bulletType;
        this.info = info;
        this.use = use;
    }
}
#endregion

public class WeaponBox : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public WeaponData data;

    #region 우클릭 Drop
    private Image dropBg;        // drop배경
    private Image dropGauge;     // drop 게이지 ( 진행도 )

    private bool isStop = false; // 클릭을 멈췄는지 확인할 변수
    #endregion

    #region F 스왑
    private Image weaponImage;               // 무기 이미지
    private Image bulletTypeImage;           // 무기 탄약 종류 이미지

    private TextMeshProUGUI weaponName;      // 무기 이름
    private TextMeshProUGUI damageText;      // 무기 대미지
    private TextMeshProUGUI bulletSizeText;  // 무기 탄창 용량
    private TextMeshProUGUI criticalText;    // 무기 치명타 확률
    private TextMeshProUGUI bulletTypeText;  // 무기 탄약 종류 텍스트
    private TextMeshProUGUI infoText;        // 무기 설명
    private TextMeshProUGUI useText;         // 사용 무기 텍스트
    #endregion

    private void Awake()
    {
        SettingUI();
    }

    private void SettingUI()
    {
        #region 우클릭 Drop
        dropBg = transform.Find("DropBg").GetComponent<Image>();
        dropGauge = dropBg.transform.Find("DropImage").GetComponent<Image>();

        dropBg.gameObject.SetActive(false);
        #endregion

        #region F 스왑
        weaponImage = transform.Find("WeaponBg/WeaponImage").GetComponent<Image>();
        bulletTypeImage = transform.Find("WeaponBg/WeaponTypeBg/WeaponTypeText/WeapomTypeImage").GetComponent<Image>();
        weaponName = transform.Find("WeaponName").GetComponent<TextMeshProUGUI>();
        damageText = transform.Find("WeaponStat/Damage/DamageText").GetComponent<TextMeshProUGUI>();
        bulletSizeText = transform.Find("WeaponStat/BulletSize/BulletSizeText").GetComponent<TextMeshProUGUI>();
        criticalText = transform.Find("WeaponStat/Critical/CriticalText").GetComponent<TextMeshProUGUI>();
        bulletTypeText = transform.Find("WeaponBg/WeaponTypeBg/WeaponTypeText").GetComponent<TextMeshProUGUI>();
        infoText = transform.Find("WeaponInfo").GetComponent<TextMeshProUGUI>();
        useText = transform.Find("isUse").GetComponent<TextMeshProUGUI>();
        #endregion
    }

    #region 우클릭 Drop
    /// <summary>
    /// 마우스를 눌렀을 때 이벤트 ( Drop )
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        // 좌클릭은 실행안되게 설정
        if ( eventData.button == PointerEventData.InputButton.Left )  // 모르면 외워 
        {
            return;
        }

        isStop = false;
        StartCoroutine(OnDropGauge());
    }

    /// <summary>
    /// 마우스를 눌렀을 때 들어가는 코루틴 함수 ( Drop )
    /// </summary>
    private IEnumerator OnDropGauge()
    {
        dropBg.gameObject.SetActive(true);
        while ( dropGauge.fillAmount < 1 )
        {
            if ( isStop )
            {
                yield break;
            }

            dropGauge.fillAmount += Time.deltaTime;

            yield return null;
        }

        yield break;
    }

    /// <summary>
    /// 마우스를 뗐을 때 이벤트 ( Drop )
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        isStop = true;
        StartCoroutine(OffDropGauge());
    }

    /// <summary>
    /// 마우스를 뗐을 때 들어가는 코루틴 함수 ( Drop )
    /// </summary>
    private IEnumerator OffDropGauge()
    {
        while ( dropGauge.fillAmount > 0)
        {
            if (!isStop)
            {
                yield break;
            }

            dropGauge.fillAmount -= Time.deltaTime;

            yield return null;
        }

        dropBg.gameObject.SetActive(false);

        yield break;
    }

    /// <summary>
    /// 게임 오브젝트가 비활성화 되었을 때 실행되는 함수 ( Drop )
    /// </summary>
    private void OnEnable()
    {  // 1회성이 아닌 비활성화 될 때마다 계속 실행되는 함수이다.
        if ( dropGauge?.fillAmount > 0 )
        {
            dropGauge.fillAmount = 0;
            dropBg.gameObject.SetActive(false);
        }
    }
    #endregion

    #region F 스왑
    /// <summary>
    /// WeaponData세팅 함수
    /// </summary>
    /// <param name="otherData">바꿀 WeaponData</param>
    public void SetData(WeaponData otherData)
    {
        data = otherData;

        weaponName.text = data.name;
        damageText.text = data.damage.ToString();
        bulletSizeText.text = data.bulletSize.ToString();
        criticalText.text = data.criticalPer.ToString();
        infoText.text = data.info;
        useText.text = data.use;
    }
    #endregion
}
