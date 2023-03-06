using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private Collider2D deathCollider;
    private Rigidbody2D deathbody;

    void Awake()
    {
        // Obtener el componente Collider del objeto actual
        deathCollider = GetComponent<Collider2D>();
        
        // Verificar si el componente Collider2D es nulo y lo crea
        if (deathCollider == null)
        {
            // Si el objeto no tiene un Collider, agregamos uno
            deathCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        // Verificar si el componente Rigidbody2D es nulo y lo crea.
        if (deathbody == null)
        {
            deathbody = gameObject.AddComponent<Rigidbody2D>();
            deathbody.gravityScale = 0f; //para que la DeathZone no caiga
        }

        // Configurar el Collider para que actúe como trigger
        deathCollider.isTrigger = true;
    }
    // Termina el chorote innecesario del awake si hubiera configurado el objeto desde el inspector


    //  Este trigger Exit está para que los objetos desaparescan una vez hayan salido completamente del escenario :)
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Desactiva el objeto que salió del contacto con la zona de muerte
        collision.gameObject.SetActive(false);
    }

    // Y este trigger enter es para el personaje, ese sí es cuando entre luego luego alv
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        collision.gameObject.SetActive(false);  // Desactiva al personaje
    }

}
