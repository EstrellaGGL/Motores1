using UnityEngine;
using UnityEngine.SceneManagement;

public class CartelScene : MonoBehaviour
{
    [SerializeField] private string LoadScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Cargando escena: " + LoadScene);
            SceneManager.LoadScene(LoadScene);
        }
    }
}