using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        Transform player = FindObjectOfType<playerHp>().GetComponent<Transform>();
        player.transform.position = this.transform.position;
    }

}
