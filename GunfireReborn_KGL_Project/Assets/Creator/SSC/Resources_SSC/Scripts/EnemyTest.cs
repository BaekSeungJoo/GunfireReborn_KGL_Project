using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyTest : MonoBehaviour
{
    public int health = 30;

    public TMP_Text currentText;

    private void Start()
    {
        currentText = transform.Find("HealthUi").transform.Find("CurrentHealth").GetComponent<TMP_Text>();
        Debug.Log(currentText.name);
    }

    public void OnDamageable(int damage)
    {
        health -= damage;

        currentText.text = health.ToString();
    }
}
