using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEffectDestroy : MonoBehaviour
{
    public float life = 0.5f;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > life)
        {
            gameObject.SetActive(false);
            timer = 0f;
        }

    }
}
