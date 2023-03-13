using System.Collections;
using UnityEngine;

public class Bandit : MonoBehaviour
{   // Sólo es para animación de muerte del enemigo Bandit
    // Animación donde muere
    private Animator banditAnimation;
    private Collider2D banditCollider;
  
    void Start()
    {
        banditAnimation = GetComponent<Animator>();
        banditCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            banditAnimation.SetBool("isDeath", true); //Le da la bala y muere :(
        }
    }

    // Método para que el colider del enemigo se actualize con la animación de muerte y no haga sus mamadas
     
    public void UpdateCollider()
    {
        // Desactivar el collider actual
        banditCollider.enabled = false;

        // Actualizar la posición del collider para que coincida con la posición actual del objeto
        banditCollider.transform.position = transform.position;

        // Activar el collider actualizado
        banditCollider.enabled = true;
    }

}
