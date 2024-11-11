using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour
{
    public Sprite HearthFull;
    public Sprite HearthHalf;
    public Image heartImage;  // Reference to the Image component in HealthUI

    private PlayerStats playerStats;

    void FixeUpdate()
    {
        // Use FindFirstObjectByType to find the PlayerStats component
        playerStats = Object.FindFirstObjectByType<PlayerStats>();

        // Update the health image when the game starts
        UpdateHealthImage();
    }

    public void UpdateHealthImage()
    {
        // Check if health is above or below half
        if (playerStats.health >= playerStats.maxHealth / 2)
        {
            heartImage.sprite = HearthFull;  // Show full heart if health is 50% or more
        }
        else
        {
            heartImage.sprite = HearthHalf;  // Show half heart if health is below 50%
        }
    }
}
