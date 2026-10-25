using UnityEngine;

public class CompanionFollow : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform playerTarget; // Arrastrá acá al Player

    [Header("Ajustes de Vuelo")]
    public float velocidad = 5f; // Qué tan rápido vuela

    // Desfase local respecto al Player (X negativo es la izquierda del cuerpo)
    public Vector3 offset = new Vector3(-0.6f, 1.4f, -0.2f);

    void Start()
    {
        if (playerTarget == null) return;

        // 1. FORZAR POSICIÓN DE INICIO: En el cuadro 1 se teletransporta directo al hombro
        Vector3 posicionInicial = playerTarget.TransformPoint(offset);
        transform.position = posicionInicial;

        // Alineamos también su rotación inicial con la del jugador
        transform.rotation = playerTarget.rotation;
    }

    void LateUpdate()
    {
        if (playerTarget == null) return;

        // 2. SEGUIMIENTO FLUIDO: El resto del tiempo te persigue suavemente por la torre
        Vector3 posicionDestino = playerTarget.TransformPoint(offset);

        // Desplazamiento por el aire e imán de rotación
        transform.position = Vector3.Lerp(transform.position, posicionDestino, velocidad * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, playerTarget.rotation, velocidad * Time.deltaTime);
    }
}

