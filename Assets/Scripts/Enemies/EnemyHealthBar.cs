using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthBarSlider; // The health bar slider UI component
    [SerializeField] private EnemyStats enemyStats; // Reference to the enemy stats script

    void Start()
    {
        // Initialize the health bar at the start
        UpdateHealthBar();
    }


    public void UpdateHealthBar()
    {
        // Calculate the health percentage
        float hpPercent = (float)enemyStats.health / enemyStats.maxHealth;

        // Update the slider value
        healthBarSlider.value = hpPercent;
    }
}
