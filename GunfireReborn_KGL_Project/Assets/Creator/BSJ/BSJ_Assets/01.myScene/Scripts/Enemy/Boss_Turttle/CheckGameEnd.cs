using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckGameEnd : MonoBehaviour
{
    public bool isGameEnd = false;
    public GameObject gameEndDoor;
    public GameObject clearText;
    public GameObject volcanoImage;

    public void Update()
    {
        if (!isGameEnd) { return; }

        else if (isGameEnd) 
        { 
            gameEndDoor.SetActive(true);
            clearText.SetActive(true);
            volcanoImage.SetActive(false);
        }
    }
}
