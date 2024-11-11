using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour
{
    public Sprite HearthFull;
    public Sprite HearthThreeQuarters;
    public Sprite HearthHalf;
    public Sprite HearthQuarter;
    public Sprite HearthEmpty;

    public Image heartImage;  // Reference to the Image component in HealthUI

    private PlayerStats playerStats;

    void FixedUpdate()
    {
        // Use FindFirstObjectByType to find the PlayerStats component
        playerStats = Object.FindFirstObjectByType<PlayerStats>();

        // Update the health image when the game starts
        UpdateHealthImage();
    }

    public void UpdateHealthImage()
    {
        // Check the player's health and update the heart image accordingly
        if (playerStats.health >= (playerStats.maxHealth * 76) / 100)
        {
            heartImage.sprite = HearthFull;
        }
        else if (playerStats.health >= (playerStats.maxHealth * 51) / 100)
        {
            heartImage.sprite = HearthThreeQuarters;
        }
        else if (playerStats.health >= (playerStats.maxHealth * 26) / 100)
        {
            heartImage.sprite = HearthHalf;
        }
        else if (playerStats.health >= (playerStats.maxHealth * 1) / 100)
        {
            heartImage.sprite = HearthQuarter;
        }
        else
        {
            heartImage.sprite = HearthEmpty;  
        }
    }
}
