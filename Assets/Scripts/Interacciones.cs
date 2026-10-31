using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class Interacciones : MonoBehaviour
{
    public TextMeshProUGUI textoInteraccion;
    public RawImage imagenInteraccion;

    private bool puedeInteractuar = false;
    private InteraccionObject objetoActual;

    private int indicePanel = 0;

    private void Start()
    {
        CerrarPaneles();
    }

    public void OnInteract()
    {
        if (puedeInteractuar && objetoActual != null)
        {
            CerrarPaneles();

            PanelInformacion informacion = objetoActual.paneles[indicePanel];

            informacion.panel.SetActive(true);
            textoInteraccion.text = informacion.texto;
            imagenInteraccion.texture = informacion.imagen;
        }
    }

    private void CerrarPaneles()
    {
        if (objetoActual != null)
        {
            foreach (PanelInformacion informacion in objetoActual.paneles)
            {
                informacion.panel.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactuable"))
        {
            objetoActual = other.GetComponent<InteraccionObject>();

            if (objetoActual != null)
            {
                puedeInteractuar = true;
                indicePanel = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactuable"))
        {
            CerrarPaneles();

            puedeInteractuar = false;
            objetoActual = null;
        }
    }
}