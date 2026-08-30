using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    float startTime;
    float finalTime;
    public MoverPersonaje player;

    void Start()
    {
        PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex);
        Debug.Log("INICIA EL JUEGO!");
        startTime = Time.time;


    }


    public void Win()
    {
        Debug.Log("GANASTE!");

        finalTime = Time.time - startTime;
        Debug.Log("Tiempo demorado: " + finalTime);
        SceneManager.LoadScene("Win");
    }

    public void Lose()
    {
        Debug.Log("PERDISTE");
        SceneManager.LoadScene("Lose");

    }


}
