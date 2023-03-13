using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float velocityMov = 2.5f;
    private Vector2 initialPosition;
    [SerializeField] static private float firstRange, secondRange;  //Rangos para número aleatorio, estáticos por que en todos los movimientos se va a aplicar igual.
    [SerializeField] public static bool easy, medium, hard, ultraHard, firstCall;  // firstCall ayudará a que empiece el juego en fácil un rato

    void Start()
    {
        Time.timeScale = 0.45f;
        initialPosition = transform.position;
        easy = true; medium = false; hard = false; ultraHard = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-velocityMov * Time.deltaTime, 0, 0);
       
        // El siguiente if indica que si el canvas está en pausa entonces no aplique la dificultad que modifica justo el tiempo.
        if (!CanvasManager.start && !CanvasManager.pause && !CanvasManager.gameOver) 
        Difficulty();
    }

    private void ResetObject()
    {
        gameObject.SetActive(true);
        transform.position = initialPosition;
    }

    private void OnDisable()
    {
        float randomDelay = Random.Range(firstRange, secondRange);
        Invoke("ResetObject", randomDelay);
    }

    // La función dificultad hace que los objetos se muevan y aparezcan más rápido
    private void Difficulty()
    {
        if (easy)
        {
            Time.timeScale = 0.45f;
            firstRange = 3.5f; secondRange = 4.5f;
        }    
        if (medium)
        {
            Time.timeScale = 0.55f;
            firstRange = 2.5f; secondRange = 3.5f;
        }
        if (hard)
        {
            Time.timeScale = 0.7f;
            firstRange = 1.5f; secondRange = 2.5f;
        }
        if (ultraHard)
        {
            Time.timeScale = 0.8f;
            firstRange = 0.5f; secondRange = 1.5f;
        }
    }

    // ESTA función pública la mando llamar desde el botón Start, agregaré objeto Buildings que la contiene para que funcione; cambia la dificultad el juego cada cierto tiempo invocandose a sí misma hasta que quede en ultra difícil
    public void ChangeDifficulty()
    {
        if (firstCall) // Primera llamada para que empiece en fácil
        {
            Invoke("ChangeDifficulty", 30f); 
            firstCall = false;
        }
        if (easy && !firstCall) // Aquí no va a hacer el cambio a menos que la primera llamada ya se haya usado
        {
            easy = false;
            medium = true;
            Invoke("ChangeDifficulty", 60f);
        } else if (medium)
        {
            medium = false;
            hard = true;
            Invoke("ChangeDifficulty", 120f);
        } else if (hard)
        {
            hard = false;
            ultraHard = true;
        }
    } 

}
