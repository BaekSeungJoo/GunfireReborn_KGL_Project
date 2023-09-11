using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private float speed = 5f;

    private bool isShop = false;

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position += Time.deltaTime * speed * new Vector3(h, 0f, v);

        if (isShop && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("상점 누름");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Shop"))
        {
            isShop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isShop = false;
    }
}
