
using System.Collections;
using UnityEngine;

public class DancerEnemy : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float cooldown = 10f;

    [Header("Referencias")]
    [SerializeField] private BoxCollider dancerCollider;
    [Header("Audio")]
    [SerializeField] private AudioSource danceAudio;
    [SerializeField] private float audioDuration = 5f;

    private void Awake()
    {
        if (dancerCollider == null)
            dancerCollider = GetComponent<BoxCollider>();

        if (danceAudio == null)
            danceAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Animator playerAnimator = other.GetComponentInChildren<Animator>();

        if (playerAnimator == null)
            return;

        // Activar el baile del jugador.
        playerAnimator.SetTrigger("Dance");

        // Activar el baile del enemigo.
        Animator enemyAnimator = GetComponent<Animator>();

        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("EnemyDance", true);
        }

        if (danceAudio != null)
        {
            danceAudio.Play();
            StartCoroutine(StopDanceAudio());
        }

        Debug.Log("Dance Activado");

        // Desactivar el collider temporalmente.
        dancerCollider.enabled = false;

        // Iniciar tiempo de recuperación.
        StartCoroutine(ReactivateCollider());
    }

    private IEnumerator ReactivateCollider()
    {
        yield return new WaitForSeconds(cooldown);

        dancerCollider.enabled = true;
    }

    private IEnumerator StopDanceAudio()
    {
        yield return new WaitForSeconds(audioDuration);

        danceAudio.Stop();
    }
}
