using UnityEngine;

public class Hud_DangerZone : MonoBehaviour
{
    [Tooltip("Cantidad de vida que resta al tocar la zona (debe ser negativo)")]
    public float danio = -25f;

    private void OnTriggerEnter(Collider other)
    {
        // Buscamos de forma desacoplada la interfaz HUD en la escena
        HUD_life hudVida = FindObjectOfType<HUD_life>();

        // Verificamos que el objeto tenga el tag Player y que el HUD exista
        if (other.CompareTag("Player") && hudVida != null)
        {
            hudVida.ModificarVida(danio);
        }
    }
}
