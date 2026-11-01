using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] private List<Transform> puntosPatrulla = new List<Transform>();
    [SerializeField] private float distanciaLlegada = 0.3f;
    [SerializeField] private float speed = 3f;

    private NavMeshAgent agent;
    private Animator animator;
    private int indiceActual = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = speed;

        if (puntosPatrulla.Count > 0)
        {
            agent.SetDestination(puntosPatrulla[indiceActual].position);
        }
    }

    private void Update()
    {
        if (puntosPatrulla.Count == 0)
            return;

        // Comprobar si el enemigo está caminando.
        bool caminando = !agent.isStopped &&
                         agent.hasPath &&
                         agent.remainingDistance > agent.stoppingDistance;

        animator.SetBool("Walk", caminando);

        // Comprobar si llegó al punto de patrulla.
        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance + distanciaLlegada)
        {
            indiceActual++;

            if (indiceActual >= puntosPatrulla.Count)
            {
                indiceActual = 0;
            }

            agent.SetDestination(puntosPatrulla[indiceActual].position);
        }
    }
}
