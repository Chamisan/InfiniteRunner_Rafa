using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    //private Collider2D pickupCollider;
    [SerializeField] private bool ifhealth;
    [SerializeField] private bool ifscore;
    [SerializeField] private bool ifammu;

    // Estos son componentes estáticos para que con cualquiera de los objetos se sume a la misma variable y no haya muchas todas raras
    public static int score = 0;
    public static int health = 3;
    public static int ammu = 10;

    // Start is called before the first frame update
    void Start()
    {
        //pickupCollider = GetComponent<Collider2D>();
        score = 0;

    }

    // Si colisiona con Player se desactiva el objeto y genera salud, score o municiones
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.gameObject.SetActive(false);
            if (ifscore)
                score++;
            if (ifhealth)
                health++;
            if (ifammu)
                ammu++;
        }
    }

}
