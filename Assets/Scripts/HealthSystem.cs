using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // Evento estático: (vidaActual, vidaMaxima)
    public static event Action<float, float> OnHealthChanged;

    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private GameManager gameManager;

    void Start()
    {
        currentHealth = maxHealth;
        gameManager = FindFirstObjectByType<GameManager>();
        NotificarCambioVida();
    }

    void Update()
    {
        // Estrategia inicial: El tiempo consume vida de forma independiente (ej: 2 puntos por segundo)
        TakeDamage(2f * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        NotificarCambioVida();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void HealMaximum()
    {
        currentHealth = maxHealth;
        NotificarCambioVida();
    }

    private void NotificarCambioVida()
    {
        // Si hay alguien escuchando (la UI), le avisa
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        if (gameManager != null)
        {
            gameManager.Lose(); // Reutiliza la lógica de derrota actual de tu proyecto
        }
    }
}
