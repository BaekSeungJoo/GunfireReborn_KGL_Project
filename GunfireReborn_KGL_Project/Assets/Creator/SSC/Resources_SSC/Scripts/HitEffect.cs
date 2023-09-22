using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private WaitForSeconds poolingTime;

    private void Awake()
    {
        poolingTime = new WaitForSeconds(1f);
    }


    private void OnEnable()
    {
        StartCoroutine(PoolingObj(P_PoolObjType.PISTOL_EFFECT));
    }

    IEnumerator PoolingObj(P_PoolObjType type)
    {
        yield return poolingTime;
        PhotonPoolManager.P_instance.CoolObj(this.gameObject, type);
    }
}
