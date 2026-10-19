using UnityEngine;
using UnityEngine.UI;

public class HUD_life : MonoBehaviour
{
    [Header("Componentes de la Interfaz")]
    public Slider sliderVida;
    public Image imagenHuevo;

    [Header("Sprites del Huevo (Estados)")]
    public Sprite huevoSaludable; // Carita feliz original
    public Sprite huevoAgrietado;  // Carita preocupada
    public Sprite huevoRoto;       // Carita de derrota

    [Header("Configuración de Vida")]
    public float vidaMaxima = 100f;
    private float vidaActual;

    [Header("Mecánica de Tiempo")]
    [Tooltip("Cantidad de vida que pierde el jugador por cada segundo que pasa")]
    public float danioPorSegundo = 5f;
    public bool disminuirPorTiempo = true;

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

    void Update()
    {
        // Si la opción está activa y el personaje sigue vivo, descuenta vida por segundo
        if (disminuirPorTiempo && vidaActual > 0)
        {
            // Usamos Time.deltaTime para que reste de forma suave y constante sin importar los FPS
            ModificarVida(-danioPorSegundo * Time.deltaTime);
        }
    }

    // Mantenemos la función pública por si a futuro decidimos implementar usar pociones o curaciones
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
            Debug.Log("El personaje se quedó sin vida por tiempo.");
            disminuirPorTiempo = false; // Frenamos el contador al morir
            // Aquí se disparará el Game Over o el Respawn
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
