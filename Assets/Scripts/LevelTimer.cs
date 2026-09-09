using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public float tiempoLimite = 60f;

    public TextMeshProUGUI textoTiempo;

    private float tiempoRestante;
    private GameManager gameManager;
    private bool terminado = false;

    private void Start()
    {
        tiempoRestante = tiempoLimite;

        gameManager = FindFirstObjectByType<GameManager>();

        ActualizarTexto();
    }

    private void Update()
    {
        if (terminado)
            return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            terminado = true;

            ActualizarTexto();

            gameManager.Lose();

            return;
        }

        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);

        textoTiempo.text = "TIEMPO RESTANTE " + minutos.ToString("00") + ":" + segundos.ToString("00");
    }
}