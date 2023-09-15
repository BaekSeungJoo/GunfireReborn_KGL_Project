using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponBox : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image dropBg;
    private Image dropGauge;

    private bool isStop = false;

    private void Start()
    {
        dropBg = transform.Find("DropBg").GetComponent<Image>();
        dropGauge = dropBg.transform.Find("DropImage").GetComponent<Image>();

        dropBg.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isStop = false;
        StartCoroutine(OnDropGauge());
    }

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

    public void OnPointerUp(PointerEventData eventData)
    {
        isStop = true;
        StartCoroutine(OffDropGauge());
    }

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
}
