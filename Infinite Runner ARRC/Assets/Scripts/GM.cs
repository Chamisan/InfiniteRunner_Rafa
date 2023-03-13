using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    //La siguiente línea logra que la clase GM no necesite instanciar un objeto para utilizar sus métodos
    public static GM Instance { get; private set; }
    public static int points;
    public static int maxPoints; 

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

        maxPoints = PlayerPrefs.GetInt("Maxpoints", 0); //Cargar el valor de maxPoints desde PlayerPrefs
        points = 0; //Se reinician los puntos cada que cargue la escena desde cero
    }
    //De aquí para adelante empiezan los métodos generales del Game Manager:

    public static void PlayGame()
    {
        Time.timeScale = 0.7f;
    }
    public static void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public static void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public static void ExitGame()
    {
        Application.Quit();
    }

}
