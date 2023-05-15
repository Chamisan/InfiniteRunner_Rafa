using UnityEngine;


public class EnemyMovement : MonoBehaviour
{

    private Rigidbody2D enemybody;
    [SerializeField] private float moveForce;
    
    //Comienza dándole un cuerpo rígido al enemigo
    void Start()
    {
        enemybody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        enemybody.AddForce(Vector2.right * moveForce * Time.deltaTime * -1, ForceMode2D.Impulse);
    }

}
