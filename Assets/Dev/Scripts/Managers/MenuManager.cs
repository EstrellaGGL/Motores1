using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public GameManager GameManager;

    public void Play()
    {
        Debug.Log("JUGAR");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void Quit()
    {
        Debug.Log("CLICK SALIR");
        Debug.Log("SALIENDO DEL JUEGO");
    }

    public void Tutorial()
    {
        Debug.Log("CLICK TUTORIAL");
        SceneManager.LoadScene("Tutorial");
    }

    public void Credits()
    {
        Debug.Log("CLICK CREDITS");
        SceneManager.LoadScene("Credits");
    }

    public void Continue()
    {
        Debug.Log("Click CONTINUAR");
        int lastLevel = PlayerPrefs.GetInt("LastLevel");   //Esto es la escena anterior a la pantalla de victoria
        int nextSceneIndex = lastLevel + 1;                //Acá se fija que nivel sigue y lo carga


        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {

            SceneManager.LoadScene(0);
        }
    }

    public void Restart()
    {
        Debug.Log("Click REINICIAR");
        int previousLevel = PlayerPrefs.GetInt("LastLevel");
        SceneManager.LoadScene(previousLevel);


    }

    public void Return()
    {
        Debug.Log("Click VOLVER");
        SceneManager.LoadScene("MainMenu");
    }



}
