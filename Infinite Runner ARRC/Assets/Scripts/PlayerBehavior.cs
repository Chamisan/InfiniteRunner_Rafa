using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerBehavior : MonoBehaviour
{
    // Para el HUD: Vidas y Municiones
    int numLifes;
    [SerializeField] GameObject[] lifes;
    [SerializeField] GameObject ammunition;
    [SerializeField] Sprite[] spritesAmmu;
    private SpriteRenderer spriteRendererAmmu;
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

    // Nueva función, balas:
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firePoint;
    int maxBullets = 5;
    int currentBullets;

    // Comienza por darle cuerpo rígido al cuerpo del caballo
    void Start()
    {
        //Time.timeScale = 0.40f; // Repetido a ver si funciona desde el principio
        horsebody = gameObject.GetComponent<Rigidbody2D>();
        isGrounded = false; //Empieza en el suelo
        numLifes = 3; //Empieza con 3 vidas
        isEdge = false; //Aún no llega al borde
        currentBullets = maxBullets; //Máximo de balas 5
        spriteRendererAmmu = ammunition.GetComponent<SpriteRenderer>(); //Obtengo el componente de Sprite desde el objeto Ammunition
        spriteRendererAmmu.sprite = spritesAmmu[currentBullets]; //Sprite del Ammu del HUD
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

        // Para las balas:
        if (collision.CompareTag("Ammu"))
        {
            collision.gameObject.SetActive(false);
            if (currentBullets < maxBullets)
            {
                currentBullets++;
                spriteRendererAmmu.sprite = spritesAmmu[currentBullets];  //Y actualiza el sprite del HUD Ammu
            }
            else GM.points += 100;
        }
    }


    // Los dos métodos siguientes ayudan a saber si está el personaje en el piso o no:
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))  //Este if es para el suelo
            isGrounded = true;

        if (collision.gameObject.CompareTag("LimitZone"))
            isEdge = true;

        // Si choca con el enemigo (Y el enemigo no está muerto), pierde una vida y se desactiva un corazón del arreglo de vidas.
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            Animator animBandit = collision.gameObject.GetComponent<Animator>();
            if (animBandit != null && !animBandit.GetBool("isDeath"))  // Si en el animator isDeath es falso entonces que haga daño
            {
                lifes[numLifes - 1].SetActive(false);  //Desactivamos corazón
                numLifes--;
                // Si el objeto no está parpadeando, comienza el flash de daño
                if (!isFlashing)
                {
                    StartCoroutine(FlashDamage());
                }
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
        if(!isEdge) //Si es que no está en el borde entonces que corra 
        horsebody.AddForce(Vector2.right * moveForce * Time.deltaTime, ForceMode2D.Impulse);

        // Aquí agregaré 1 punto de score por cada 0.1 segundos que pasen 
        if (Time.time - lastPointTime >= 0.2f)
        {
            GM.points += 1;
            lastPointTime = Time.time; //Actualiza el tiempo en el que se sumó el último punto
        }  

        // Si se queda sin vidas el player se desactiva (muere)
        if (numLifes <= 0)
            gameObject.SetActive(false);

        // Comportamiento de disparo: Si Presiona disparo, hay balas y el menú pausa está desactivado, que dispare
        if (Input.GetButtonDown("Fire1") && currentBullets > 0 && !CanvasManager.activeCanvas.activeSelf)
        {
            GameObject bulletObject = Instantiate(bullet, firePoint.position, firePoint.rotation);
            currentBullets--;
            spriteRendererAmmu.sprite = spritesAmmu[currentBullets]; //Actualiza el sprite Ammu del HUD
            //Destroy(bulletObject, 1f); Esta es la manera correcta, destruir el objeto bulletObject y no la instancia
        }
    }

    // Por último si el Player es desactivado, se acaba el juego:
    private void OnDisable()
    {
        CanvasManager.gameOver = true;
    }


}
