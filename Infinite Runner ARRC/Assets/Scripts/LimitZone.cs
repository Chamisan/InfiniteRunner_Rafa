using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitZone : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Active el stop de personaje.
            
        }
    }

}
