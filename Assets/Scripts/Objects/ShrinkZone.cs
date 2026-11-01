using StarterAssets;
using UnityEngine;


 

public class ShrinkZone : MonoBehaviour  //Zona que detecta cuando el jugador entra/sale y activa/desactiva el estado de shrink.
{
    
    private void OnTriggerEnter(Collider other) // Si es el jugador, setea _inShrinkZone = true para que se mantenga pequeño.
    {
        
        if (other.CompareTag("Player")) // Solo actuar si el objeto que entra tiene el tag "Player"
        {
            
            ThirdPersonController player = other.GetComponent<ThirdPersonController>(); // Obtener el script ThirdPersonController del jugador
            if (player != null)
            {
                
                player._inShrinkZone = true; // Activar el estado de shrink zone
            }
        }
    }

    private void OnTriggerExit(Collider other) //Si es el jugador, setea _inShrinkZone = false para que pueda agrandarse
    {
        
        if (other.CompareTag("Player")) // Solo actuar si el objeto que sale tiene el tag "Player"
        {
            
            ThirdPersonController player = other.GetComponent<ThirdPersonController>(); // Obtener el script ThirdPersonController del jugador
            if (player != null)
            {
                
                player._inShrinkZone = false; // Desactivar el estado de shrink zone
            }
        }
    }
}