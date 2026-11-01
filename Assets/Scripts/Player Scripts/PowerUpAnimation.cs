
using System.Collections;
using UnityEngine;

public class PowerUpAnimation : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator animator;

    [Header("Animaciones de PowerUp")]
    [SerializeField] private AnimationClip[] powerUpClips;

    [Header("Capas del Animator")]
    [SerializeField] private string powerUpLayerName = "PowerUp Layer";

    private int powerUpLayerIndex;
    private Coroutine playingCoroutine;
    public bool IsPlayingPowerUp { get; private set; }

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        // Busca la capa por su nombre.
        powerUpLayerIndex = animator.GetLayerIndex(powerUpLayerName);

        if (powerUpLayerIndex < 0)
        {
            Debug.LogError("No se encontró la capa: " + powerUpLayerName);
            enabled = false;
            return;
        }

        // Estado inicial: Base Layer activa y PowerUp apagada.
        animator.SetLayerWeight(1, 1f);
        animator.SetLayerWeight(powerUpLayerIndex, 0f);
    }

    public void PlayRandomPowerUp()
    {
        Debug.Log("PowerUp");
        if (powerUpClips == null || powerUpClips.Length == 0)
            return;

        int index = Random.Range(0, powerUpClips.Length);

        if (powerUpClips[index] == null)
            return;

        // Si ya se estaba reproduciendo otro power-up, lo interrumpe.
        if (playingCoroutine != null)
            StopCoroutine(playingCoroutine);

        playingCoroutine = StartCoroutine(PlayPowerUp(index));
    }

    private IEnumerator PlayPowerUp(int index)
    {
        IsPlayingPowerUp = true;
        AnimationClip clip = powerUpClips[index];

        Debug.Log("Iniciando power-up: PowerUp" + (index + 1));

        // Desactiva visualmente la Base Layer.
        animator.SetLayerWeight(1, 0f);

        // Activa la capa PowerUp Layer.
        animator.SetLayerWeight(powerUpLayerIndex, 1f);

        // Reproduce el estado correcto desde el inicio.
        string estado = "PowerUp" + (index + 1);

        int hashEstado = Animator.StringToHash(estado);

        Debug.Log("Estado: " + estado +
                  " | Layer: " + powerUpLayerIndex);

        animator.Play(hashEstado, powerUpLayerIndex, 0f);

        // Espera un frame para iniciar la reproducción.
        yield return null;

        // Espera la duración del clip.
        yield return new WaitForSeconds(
            clip.length / Mathf.Max(animator.speed, 0.01f)
        );

        // Devuelve los pesos a su estado normal.
        animator.SetLayerWeight(powerUpLayerIndex, 0f);
        animator.SetLayerWeight(1, 1f);
        IsPlayingPowerUp = false;

        Debug.Log("Power-up finalizado. Base Layer restaurada.");

        playingCoroutine = null;
    }
}
