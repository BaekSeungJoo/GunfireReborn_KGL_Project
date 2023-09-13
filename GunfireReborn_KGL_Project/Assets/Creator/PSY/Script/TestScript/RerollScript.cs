using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RerollScript : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI rerollText;

    private void Start()
    {
        rerollText = GetComponent<TextMeshProUGUI>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        rerollText.text = "새로 고침 <i>0 / 1 </i>";
    }
}
