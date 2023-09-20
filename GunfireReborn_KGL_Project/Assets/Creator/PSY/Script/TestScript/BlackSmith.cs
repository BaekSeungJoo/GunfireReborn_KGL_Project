using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmith : MonoBehaviour
{
    private TextMeshProUGUI weaponName;   // 무기 이름
    private Button bt_Upgrade;            // 강화 버튼
    private TextMeshProUGUI upgradeCoin;  // 강화 코인 
    private TextMeshProUGUI upgradeCount; // 남은 강화 횟수

    private int weaponUpgradeCount = 0;   // 현재 무기의 강화 수치
    private int upgradePrice = 0;       // 현재 강화 비용

    #region 프로퍼티
    public int WeaponUpgradeCount /* 현재 무기의 강화 수치 프로퍼티 */
    { get { return weaponUpgradeCount; } set { weaponUpgradeCount = value; } }


    public int UpgradePrice /* 현재 강화 비용 프로퍼티 */
    { get { return upgradePrice; } set { upgradePrice = value; } }
    #endregion


    private string weapon = "불꽃비늘";

    private void Start()
    {
        weaponName = transform.Find("WeaponName").GetComponent<TextMeshProUGUI>();
        bt_Upgrade = transform.Find("EnhanceButton").GetComponent<Button>();
        upgradeCoin = bt_Upgrade.transform.Find("UpgradeCoin").GetComponent<TextMeshProUGUI>();
        upgradeCount = transform.parent.Find("UpgradeCount").GetComponent<TextMeshProUGUI>();
    }

    #region 세팅 함수
    /// <summary>
    /// 잔여 강화 횟수 Text 세팅 함수
    /// </summary>
    /// <param name="count">강화 잔여 횟수</param>
    public void SetUpgradeCount(int count)
    {
        upgradeCount.text = $"잔여 강화 횟수 : {count}";
    }

    /// <summary>
    /// 강화 버튼 비활성화 세팅 함수
    /// </summary>
    public void SetUpgradeBT()
    {
        bt_Upgrade.interactable = false;  // 버튼 활성화 여부
    }

    /// <summary>
    /// 무기 강화 Text 세팅 함수
    /// </summary>
    public void SetUpgradeWeaponName()
    {
        weaponName.text = $"{weapon} +{weaponUpgradeCount}";
    }

    /// <summary>
    /// 강화 코인 Text 세팅 함수
    /// </summary>
    public void SetUpgradeCoin()
    {
        upgradeCoin.text = $"{upgradePrice}";
    }
    #endregion


}
