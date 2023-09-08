using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPBar : MonoBehaviour
{
    private Canvas canvas;
    private Camera cam;
    private RectTransform rectParent;
    private RectTransform rectHp;

    // Hp바가 오브젝트로 부터 얼마나 떨어질 것인지?
    [HideInInspector]
    public Vector3 offset = Vector3.zero;
    [HideInInspector]
    public Transform targetTransform;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        cam = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = gameObject.GetComponent<RectTransform>();
    }


    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(targetTransform.position + offset);
        
        if(screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        Vector2 localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, cam, out localPos);

        rectHp.localPosition = localPos;
    }

}
