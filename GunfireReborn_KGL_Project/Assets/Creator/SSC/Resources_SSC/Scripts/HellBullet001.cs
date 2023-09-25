using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class HellBullet001 : MonoBehaviourPun
{
    private WaitForSeconds poolingTime;

    [SerializeField] private GameObject damageText;

    private TextMeshProUGUI damageSetting;

    void Awake()
    {
        poolingTime = new WaitForSeconds(5f);
        damageSetting = damageText.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(DestroyBullet(P_PoolObjType.HELLBULLET));
    }
    private IEnumerator DestroyBullet(P_PoolObjType type)
    {
        yield return poolingTime;
        PhotonPoolManager.P_instance.CoolObj(this.gameObject, type);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            damageSetting.text = "" + UpgradeManager.up_Instance.shotgunDamage;
            damageSetting.color = Color.yellow;
            Instantiate(damageText, transform.position, Quaternion.identity);

            PhotonPoolManager.P_instance.CoolObj(this.gameObject, P_PoolObjType.PISTOLBULLET);
            EnemyHealth health = other.GetComponent<EnemyHealth>();

            health.EnemyTakeDamage(UpgradeManager.up_Instance.shotgunDamage);
        }

        if (other.CompareTag("LuckyShotPoint"))
        {
            damageSetting.text = UpgradeManager.up_Instance.shotgunDamage * 2 + "!";
            damageSetting.color = Color.red;
            Instantiate(damageText, transform.position, Quaternion.identity);

            PhotonPoolManager.P_instance.CoolObj(this.gameObject, P_PoolObjType.PISTOLBULLET);
            EnemyHealth health = GFunc.FindRootObj(other.gameObject).GetComponent<EnemyHealth>();

            health.EnemyTakeDamage(UpgradeManager.up_Instance.shotgunDamage);
        }
    }

}
