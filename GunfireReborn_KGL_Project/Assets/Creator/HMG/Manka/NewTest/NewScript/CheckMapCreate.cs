using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMapCreate : MonoBehaviour
{
    public GameObject playerCreateManager;

    private void Start()
    {
        playerCreateManager.SetActive(true);
    }
}
