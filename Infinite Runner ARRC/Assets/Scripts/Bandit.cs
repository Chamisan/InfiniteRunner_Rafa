using System.Collections;
using UnityEngine;

public class Bandit : MonoBehaviour
{   // S�lo es para animaci�n de muerte del enemigo Bandit
    private Animator banditAnimation;
    [SerializeField] private Collider2D banditCollider;
  
    void Start() //Obtiene los componentes del mismo objeto
    {
        banditAnimation = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
            banditAnimation.SetBool("isDeath", true); //Le da la bala y muere :(
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            banditAnimation.SetBool("isDeath", true); //Si le cae encima el Player, muere (tiene un trigger arriba)
    } 

    // M�todo para que el colider del enemigo se actualize con la animaci�n de muerte y no haga sus mamdas
    public void UpdateCollider()
    {
        banditCollider.enabled = false; //Ahora s�lo lo dej� en activar el trigger as� desaparece en la DeathZone
        // Actualizar la posici�n del collider para que coincida con la posici�n actual del sprite del momento nuevo
        banditCollider.transform.position = transform.position;
        // Activar el collider actualizado
        //banditCollider.enabled = true; 
        // Coment� todo ya porque la idea es que no estorbe al personaje cuando muere el enemigo
    }

    private void OnDisable()
    {
        banditCollider.enabled = true;
    }

}
