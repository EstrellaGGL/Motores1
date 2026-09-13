using UnityEngine;

public class ZonaInformacion : MonoBehaviour
{
    [SerializeField] private GameObject panelInfo;

    private void Start()
    {
        panelInfo.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos que quien entró sea el Player.
        if (other.CompareTag("Player"))
        {
            panelInfo.SetActive(true);
          //  Debug.Log("Entró el Player");
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            panelInfo.SetActive(false);
           // Debug.Log("Salió el Player");
        }
    }
}