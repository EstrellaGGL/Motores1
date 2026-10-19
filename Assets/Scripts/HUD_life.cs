using UnityEngine;
using UnityEngine.UI;

public class HUD_life : MonoBehaviour
{
    [Header("Componentes de la Interfaz")]
    public Slider sliderVida;
    public Image imagenHuevo;

    [Header("Sprites del Huevo (Estados)")]
    public Sprite huevoSaludable; // Carita feliz original
    public Sprite huevoAgrietado;  // Carita preocupada o rota
    public Sprite huevoRoto;       // Carita de derrota

    [Header("Configuración de Vida")]
    public float vidaMaxima = 100f;
    private float vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
        if (sliderVida != null)
        {
            sliderVida.maxValue = vidaMaxima;
            sliderVida.value = vidaMaxima;
        }
        ActualizarExpresionHuevo();
    }

    // Cualquier peligro del mapa llamará a esta función modular
    public void ModificarVida(float cantidad)
    {
        vidaActual += cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        if (sliderVida != null)
        {
            sliderVida.value = vidaActual;
        }

        ActualizarExpresionHuevo();

        if (vidaActual <= 0)
        {
            Debug.Log("Personaje derrotado. Disparar lógica de Mixamo / Respawn.");
        }
    }

    private void ActualizarExpresionHuevo()
    {
        if (imagenHuevo == null) return;

        float porcentaje = vidaActual / vidaMaxima;

        if (porcentaje > 0.6f)
        {
            if (huevoSaludable != null) imagenHuevo.sprite = huevoSaludable;
        }
        else if (porcentaje > 0.2f)
        {
            if (huevoAgrietado != null) imagenHuevo.sprite = huevoAgrietado;
        }
        else
        {
            if (huevoRoto != null) imagenHuevo.sprite = huevoRoto;
        }
    }
}
