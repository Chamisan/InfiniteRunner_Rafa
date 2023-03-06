using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour 
{
   
    [SerializeField] Button pauseButton;  // El botón de pausa: 

    [SerializeField] TextMeshProUGUI score;  // La variable texto para el score:

    [SerializeField] TextMeshProUGUI highScore;  // La variable texto para el highScore:

    //La siguiente línea logra que esta clase no necesite instanciar un objeto para utilizar sus métodos
    public static CanvasManager Instance { get; private set; }

    //Este Awake es complementario a lo anterior, hace una instancia de esta clase desde el principio de todo.
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

    // Los siguientes bools públicos son para activar y desactivar los canvas, son públicos para usarse en otros scripts y poder pausar desde otros métodos comienza start activado :)
    public static bool start = true;
    public static bool pause = false;
    public static bool gameOver = false;

    // Estos objetos serán los canvas que estaré activando y desactivando
    private GameObject activeCanvas;
    private GameObject startCanvas;
    private GameObject pauseCanvas;
    private GameObject gameOverCanvas;

    
    // Start busca los canvas en la jerarquía y los asigna a los objetos además que da true a start para que siempre inicie en ese canvas.
    private void Start()
    {
        
        start = true; // Comienza el juego con start activado
        // Busca el objeto en la jerarquía y lo asigna 
        startCanvas = GameObject.Find("StartCanvas");        
        pauseCanvas = GameObject.Find("PauseCanvas");
        pauseCanvas.SetActive(false);
        gameOverCanvas = GameObject.Find("GameOverCanvas");
        gameOverCanvas.SetActive(false);
        activeCanvas = startCanvas; //Aquí se asigana como activo el startCanvas
        activeCanvas.SetActive(true);
        pauseButton.interactable = false;
        highScore.text = "High Score: " + GM.maxPoints.ToString("D4"); //Para que el High Score aparezca desde el principio. 
    }


    // Este método comienza el juego y desactivará todos los canvas para comenzar a jugar
    public void RestartGame()
    {
        // Jalar método de GM que Reinicia el juego.
        GM.RestartGame();
    }

    //Método que inicia el juego desactivando el canvas activo:
    public void ContinueGame()
    {
        activeCanvas.SetActive(false);
        startCanvas.SetActive(false);
        start = false;
        pause = false;
        gameOver = false;
        //Se activa el botón de pausa
        pauseButton.interactable = true;
    }

    public void QuitGame()
    {
        GM.ExitGame();
    }

    //Método que activa el canvas start
    public void StartCanvas()
    {
        pause = false;
        gameOver = false;
        start = true;
        activeCanvas = startCanvas;
        activeCanvas.SetActive(true);
        pauseButton.interactable = false;
    }

    //Método que activa el canvas pause
    public void PauseCanvas()
    {
        start = false;
        gameOver = false;
        pause = true;
        activeCanvas = pauseCanvas;
        activeCanvas.SetActive(true);
        //Se desactiva el botón de pausa
        pauseButton.interactable = false;
    }

    //Método que activa el canvas game over
    public void GameOverCanvas()
    {
        start = false;
        pause = false;
        gameOver = true;
        activeCanvas = gameOverCanvas;
        activeCanvas.SetActive(true);
        pauseButton.interactable = false;
        if (GM.points > GM.maxPoints) // Este if dice que si los puntos son mayores a los máximos entonces cambie la puntuación más alta
        {
            GM.maxPoints = GM.points;
            highScore.text = "High Score: " + GM.maxPoints.ToString("D4");
        }
    }

    void Update()
    {
        // Los siguientes if nombrarán al objeto según el bool activado y lo activarán:
        if (start)
            StartCanvas();

        if (pause)
            PauseCanvas();
        
        if (gameOver)
            GameOverCanvas();

        // Si todos están desactivados entonces que desactive el canvas activo:
        if (!start && !pause && !gameOver)
            activeCanvas.SetActive(false);

        // Si el objeto activeCanvas está activado entonces pausa y sino entonces play
        if (activeCanvas.activeSelf)
             GM.PauseGame();
        else GM.PlayGame();

        // Aquí toma el puntaje que tiene el GameManager y lo pasa con 4 dígitos al texto score
        score.text = GM.points.ToString("D4");
    }

}
