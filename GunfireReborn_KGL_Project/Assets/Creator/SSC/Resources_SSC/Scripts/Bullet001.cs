using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Bullet001 : MonoBehaviourPun
{
    private WaitForSeconds poolingTime;

    [SerializeField] private GameObject damageText;

    private TextMeshProUGUI damageSetting;

    public int riflebulletDamage;

    // Start is called before the first frame update
    void Awake()
    {
        damageSetting = damageText.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        poolingTime = new WaitForSeconds(5f);
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyBullet(P_PoolObjType.BULLET));
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
            damageSetting.text = "" + riflebulletDamage;
            damageSetting.color = Color.yellow;
            Instantiate(damageText, transform.position, Quaternion.identity);    
            PhotonPoolManager.P_instance.CoolObj(this.gameObject, P_PoolObjType.PISTOLBULLET);
            EnemyHealth health = other.GetComponent<EnemyHealth>();

            health.EnemyTakeDamage(riflebulletDamage);
        }

        if (other.CompareTag("LuckyShotPoint"))
        {
            damageSetting.text = riflebulletDamage * 2 + "!";
            damageSetting.color = Color.red;
            Instantiate(damageText, transform.position, Quaternion.identity);
            PhotonPoolManager.P_instance.CoolObj(this.gameObject, P_PoolObjType.PISTOLBULLET);

            EnemyHealth health = GFunc.FindRootObj(other.gameObject).GetComponent<EnemyHealth>();

            health.EnemyTakeDamage(riflebulletDamage * 2);
        }
    }



}
