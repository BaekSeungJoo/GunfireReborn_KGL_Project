using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckGameEnd : MonoBehaviour
{
    public bool isGameEnd = false;
    public GameObject gameEndDoor;
    public GameObject clearText;
    public GameObject volcano;
    public GameObject BossCanvas;

    public EnemyHealth boss_Health;

    public void Update()
    {
        if (boss_Health != null && boss_Health.curHealth <= 0) { isGameEnd = true; }

        if (!isGameEnd) { return; }

        else if (isGameEnd) 
        { 
            gameEndDoor.SetActive(true);
            clearText.SetActive(true);
            volcano.SetActive(false);
            BossCanvas.SetActive(false);
        }
    }
}
