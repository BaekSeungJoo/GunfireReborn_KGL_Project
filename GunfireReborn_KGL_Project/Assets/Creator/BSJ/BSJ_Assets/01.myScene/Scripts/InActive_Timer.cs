using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InActive_Timer : MonoBehaviour
{
    private float InActiveTime = 5f;
    private float Timer = 0f;

    private void Update()
    {
        Timer = Time.deltaTime;

        if (Timer > InActiveTime )
        {
            gameObject.SetActive(false);
        }
    }
}
