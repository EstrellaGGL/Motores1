using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Interacciones : MonoBehaviour
{
    public GameObject panelInteraccion;
    public TextMeshProUGUI textoInteraccion;

    private bool puedeInteractuar = false;
    private string textoActual;

    private void Start()
    {
        panelInteraccion.SetActive(false);
    }

    public void OnInteract()
    {
        if (puedeInteractuar)
        {
            textoInteraccion.text = textoActual;
            panelInteraccion.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactuable"))
        {
            InteraccionObject objeto = other.GetComponent<InteraccionObject>();

            if (objeto != null)
            {
                puedeInteractuar = true;
                textoActual = objeto.texto;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactuable"))
        {
            puedeInteractuar = false;
            panelInteraccion.SetActive(false);
        }
    }
}