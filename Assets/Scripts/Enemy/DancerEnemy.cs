using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DancerEnemy : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float cooldown = 10f;

    [Header("Referencias")]
    [SerializeField] private BoxCollider dancerCollider;

    [Header("Audio")]
    [SerializeField] private AudioSource danceAudio;
    [SerializeField] private float audioDuration = 5f;

    private NavMeshAgent agent;
    private Animator enemyAnimator;
    private bool dancing = false;

    private void Awake()
    {
        if (dancerCollider == null)
            dancerCollider = GetComponent<BoxCollider>();

        if (danceAudio == null)
            danceAudio = GetComponent<AudioSource>();

        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (dancing)
            return;

        Animator playerAnimator = other.GetComponentInChildren<Animator>();

        if (playerAnimator == null)
            return;

        PlayerDanceLock playerDance = other.GetComponentInParent<PlayerDanceLock>();

        if (playerDance == null)
            return;

        dancing = true;

        // Detener la patrulla del enemigo.
        if (agent != null)
            agent.isStopped = true;

        // Bloquear movimiento del jugador.
        playerDance.SetDancing(true);

        // Activar el baile del jugador.
        playerAnimator.SetTrigger("Dance");

        // Activar el baile del enemigo.
        if (enemyAnimator != null)
            enemyAnimator.SetBool("EnemyDance", true);

        // Reproducir audio.
        if (danceAudio != null)
            danceAudio.Play();

        Debug.Log("Dance Activado");

        // Desactivar el collider temporalmente.
        dancerCollider.enabled = false;

        StartCoroutine(ReactivateCollider());
        StartCoroutine(FinishDance(playerDance));
    }

    private IEnumerator FinishDance(PlayerDanceLock playerDance)
    {
        yield return new WaitForSeconds(audioDuration);

        // Desactivar la animación de baile del enemigo.
        if (enemyAnimator != null)
            enemyAnimator.SetBool("EnemyDance", false);

        // Detener audio.
        if (danceAudio != null)
            danceAudio.Stop();

        // Devolver el control al jugador.
        playerDance.SetDancing(false);

        // Reanudar la patrulla.
        if (agent != null)
            agent.isStopped = false;

        dancing = false;
    }

    private IEnumerator ReactivateCollider()
    {
        yield return new WaitForSeconds(cooldown);

        dancerCollider.enabled = true;
    }
}