using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerBehavior : MonoBehaviour
{
    // Para el HUD: Vidas
    int numLifes;
    [SerializeField] GameObject[] lifes;
    float lastPointTime;

    private Rigidbody2D horsebody;
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveForce;
    private bool isGrounded;
    private bool isEdge;

    //  Esta línea me permitira agregar un animator para cambiar entre salto y correr
    public Animator animator;

    // Esto es para que parpadee en rojo al recibir daño:
    public float damageFlashDuration = 0.1f; // Duración del flash de daño en segundos
    public Color damageFlashColor = Color.red; // Color del flash de daño

    private Color originalColor; // Color original del objeto
    private bool isFlashing = false; // Indica si el objeto está parpadeando

    // Comienza por darle cuerpo rígido al cuerpo del caballo
    void Start()
    {
        horsebody = gameObject.GetComponent<Rigidbody2D>();
        isGrounded = false; // Empieza en el suelo
        numLifes = 3; // Empieza con 3 vidas
        isEdge = false; // Aún no llega al borde
    }

    // Corrutina que hace que el objeto parpadee en rojo
    private IEnumerator FlashDamage()
    {
        // Guarda el color original del objeto
        originalColor = GetComponent<Renderer>().material.color;

        // Indica que el objeto está parpadeando
        isFlashing = true;

        // Alterna entre el color original y el color de flash de daño
        for (int i = 0; i < 4; i++)
        {
            GetComponent<Renderer>().material.color = damageFlashColor;
            yield return new WaitForSeconds(damageFlashDuration / 2);
            GetComponent<Renderer>().material.color = originalColor;
            yield return new WaitForSeconds(damageFlashDuration / 2);
        }

        // Indica que el objeto ya no está parpadeando
        isFlashing = false;
    }

    // Estos trigger interactúan con los pickups aumentando vidas y puntos además de desactivarlos
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Life"))
        {
            collision.gameObject.SetActive(false);
            if (numLifes < 3)
            {
                numLifes++;
                lifes[numLifes - 1].SetActive(true);  // Activamos el corazoncini del HUD
            }
            else GM.points += 100;
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            GM.points += 50;
            collision.gameObject.SetActive(false);
        }
    }


    // Los dos métodos siguientes ayudan a saber si está el personaje en el piso o no:
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))  // Este if es para el suelo
            isGrounded = true;

        if (collision.gameObject.CompareTag("LimitZone"))
            isEdge = true;

        // Si choca con el enemigo, pierde una vida y se desactiva un corazón del arreglo de vidas.
        if (collision.gameObject.CompareTag("Enemy"))
        {
            lifes[numLifes - 1].SetActive(false);  // Desactivamos corazón
            numLifes--;
            // Si el objeto no está parpadeando, comienza el flash de daño
            if (!isFlashing)
            {
                StartCoroutine(FlashDamage());
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            isGrounded = false;
    }


    // El update con todas sus chunches 
    void Update()
    {
        // Esta función hace saltar al caballo sólo si está en el piso y cambiar su animación a salto
         if(isGrounded && Input.GetButtonDown("Jump"))
         {
            animator.SetBool("jumping", true);
            horsebody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
         }

        // Este if hace que pare de saltar la animación cuando sueltas el botón salto
        if (Input.GetButtonUp("Jump"))
            animator.SetBool("jumping", false);

        // Movimiento constante hacia adelante:
        if(!isEdge) // Si es que no está en el borde entonces que corra 
        horsebody.AddForce(Vector2.right * moveForce * Time.deltaTime, ForceMode2D.Impulse);

        // Aquí agregaré 1 punto de score por cada 0.1 segundos que pasen 
        if (Time.time - lastPointTime >= 0.2f)
        {
            GM.points += 1;
            lastPointTime = Time.time; // Actualiza el tiempo en el que se sumó el último punto
        }  

        // Si se queda sin vidas el player se desactiva (muere)
        if (numLifes <= 0)
            gameObject.SetActive(false);
    }

    // Por último si el Player es desactivado, se acaba el juego:
    private void OnDisable()
    {
        CanvasManager.gameOver = true;
    }

}
