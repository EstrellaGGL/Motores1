using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private Sprite[] eggSprites; // aca se arrastrá los 5 huevos acá en orden (0 al 4)

    void OnEnable() => HealthSystem.OnHealthChanged += UpdateEggSprite;
    void OnDisable() => HealthSystem.OnHealthChanged -= UpdateEggSprite;

    private void UpdateEggSprite(float currentHealth, float maxHealth)
    {
        float pct = Mathf.Clamp01(currentHealth / maxHealth);

        // Divide el 100% de vida en 5 indices (0 a 4) de forma inversa
        int index = Mathf.Clamp(Mathf.FloorToInt((1f - pct) * eggSprites.Length), 0, eggSprites.Length - 1);

        healthImage.sprite = eggSprites[index];
    }
}
