using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour 
{
    [SerializeField] Button pauseButton; 
    [SerializeField] TextMeshProUGUI score, highScore;  //La variable texto para el score y el highScore

    // La siguiente l�nea logra que esta clase no necesite instanciar un objeto para utilizar sus m�todos
    public static CanvasManager Instance { get; private set; }

    // Este Awake es complementario a lo anterior, hace una instancia de esta clase desde el principio de todo.
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Los siguientes bools son para activar y desactivar los canvas, son p�blicos para usarse en otros scripts y poder pausar desde otros m�todos comienza start activado :)
    public static bool start = true, pause = false, gameOver = false;

    // Estos objetos ser�n los canvas que estar� activando y desactivando
    public static GameObject activeCanvas; //Lo usar� para saber si el juego est� en pausa para el disparo
    private GameObject startCanvas, pauseCanvas, gameOverCanvas;

    
    // Start busca los canvas en la jerarqu�a y los asigna a los objetos adem�s que da true a start para que siempre inicie en ese canvas.
    private void Start()
    {  
        start = true; //Comienza el juego con start activado
        // Busca el objeto en la jerarqu�a y lo asigna a los canvas 
        startCanvas = GameObject.Find("StartCanvas");
        pauseCanvas = GameObject.Find("PauseCanvas");
        pauseCanvas.SetActive(false);
        gameOverCanvas = GameObject.Find("GameOverCanvas");
        gameOverCanvas.SetActive(false);
        activeCanvas = startCanvas; //Aqu� se asigana como activo el startCanvas
        activeCanvas.SetActive(true);
        pauseButton.interactable = false;
        highScore.text = "High Score: " + GM.maxPoints.ToString("D4"); //Para que el High Score aparezca desde el principio. 
    }


    public void RestartGame()
    {
        GM.RestartGame(); //Jalar m�todo de GM que Reinicia el juego.
    }

    // M�todo que inicia o contin�a el juego desactivando el canvas activo:
    public void ContinueGame()
    {
        activeCanvas.SetActive(false);
        start = false;
        pause = false;
        gameOver = false;
        pauseButton.interactable = true; //Se activa el bot�n de pausa
    }

    // M�todo que activa el canvas start
    public void StartCanvas()
    {
        pause = false;
        gameOver = false;
        start = true;
        activeCanvas = startCanvas;
        activeCanvas.SetActive(true);
        pauseButton.interactable = false;
    }

    // M�todo que activa el canvas pause
    public void PauseCanvas()
    {
        start = false;
        gameOver = false;
        pause = true;
        activeCanvas = pauseCanvas;
        activeCanvas.SetActive(true);
        //Se desactiva el bot�n de pausa
        pauseButton.interactable = false;
    }

    //M�todo que activa el canvas game over
    public void GameOverCanvas()
    {
        start = false;
        pause = false;
        gameOver = true;
        activeCanvas = gameOverCanvas;
        activeCanvas.SetActive(true);
        pauseButton.interactable = false;
        if (GM.points > GM.maxPoints) // Este if dice que si los puntos son mayores a los m�ximos entonces cambie la puntuaci�n m�s alta
        {
            GM.maxPoints = GM.points;
            highScore.text = "High Score: " + GM.maxPoints.ToString("D4");
            PlayerPrefs.SetInt("Maxpoints", GM.maxPoints);
        }
    }

    void Update()
    {
        // Los siguientes if nombrar�n al objeto seg�n el bool activado y lo activar�n:
        if (start)
            StartCanvas();
        if (pause)
            PauseCanvas();
        if (gameOver)
            GameOverCanvas();

        // Si todos est�n desactivados entonces que desactive el canvas activo:
        if (!start && !pause && !gameOver)
            activeCanvas.SetActive(false);

        // Si el objeto activeCanvas est� activado entonces pausa y sino entonces play
        if (activeCanvas.activeSelf)
             GM.PauseGame();
        else GM.PlayGame();

        // Aqu� toma el puntaje que tiene el GameManager y lo pasa con 4 d�gitos al texto score
        score.text = GM.points.ToString("D4");
    }
    public void QuitGame()
    {
        GM.ExitGame();
    }
}
