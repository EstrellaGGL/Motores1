
using UnityEngine;

public class Swimming : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        // Busca el Animator en el personaje o en sus hijos.
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Al entrar en el agua, activa la animación de nado.
        if (other.CompareTag("Water"))
        {
            animator.SetBool("IsSwimming", true);
            Debug.Log("En el agua");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Al salir del agua, vuelve a la animación normal.
        if (other.CompareTag("Water"))
        {
            animator.SetBool("IsSwimming", false);
            Debug.Log("Fuera del agua");
        }
    }
}
