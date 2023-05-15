using UnityEngine;

public class Bullet : MonoBehaviour
{   // Clase Bala: S�lo tiene su movimiento, creaci�n de puntos al matar enemigos y destrucci�n al desactivarse

    [SerializeField] float bulletSpeed = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GM.points += 20;
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);
    }//Movimiento de la bala

    private void OnDisable() //Destruye la bala
    {
        Destroy(gameObject);
    }

}
