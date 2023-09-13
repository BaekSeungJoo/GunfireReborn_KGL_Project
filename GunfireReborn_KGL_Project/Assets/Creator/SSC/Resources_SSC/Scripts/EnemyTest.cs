using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyTest : MonoBehaviour
{
    public int health = 30;

    private Collider childCollider;
    public TMP_Text currentText;

    private void Start()
    {
        currentText = transform.Find("HealthUi").transform.Find("CurrentHealth").GetComponent<TMP_Text>();

        for(int i = 0; i < transform.childCount - 2;  i++)
        {
            childCollider = transform.GetChild(i).GetComponent<Collider>();
        }
    }

    public void OnDamageable(int damage)
    {
        health -= damage;
        currentText.text = "" + health;
    }
}
