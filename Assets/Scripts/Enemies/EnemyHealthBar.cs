using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    [SerializeField] private EnemyStats enemyStats;

    void Start()
    {
        // Initialize the health bar at the start
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        // Calculate the health percentage
        float hpPercent = (float)enemyStats.health / enemyStats.maxHealth;
        healthBarSlider.value = hpPercent;

        if (hpPercent >= 1f)
        {
            healthBarSlider.gameObject.SetActive(false); // Hide the slider
        }
        else
        {
            healthBarSlider.gameObject.SetActive(true); // Show the slider
        }
    }
}
