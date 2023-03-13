using UnityEngine;

public class Bullet : MonoBehaviour
{   // Aquí la clase Bala sólo tiene su movimiento, creación de puntos al matar enemigos y destrucción al desactivarse

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
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

}
