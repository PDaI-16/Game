using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public GameObject Player;
    public float health;
    public float maxHealth;
    public string playerClass;
    private BottomBar bottomBar;

    void Awake()
    {
        // Initialize player stats if a class is selected
        if (!string.IsNullOrEmpty(MainMenu.selectedClass))
        {
            InitializeStats();
        }
    }

    void Start()
    {
        // Double-check in case Awake didn't have the correct class selection
        if (string.IsNullOrEmpty(playerClass) && !string.IsNullOrEmpty(MainMenu.selectedClass))
        {
            InitializeStats();
            bottomBar = Object.FindFirstObjectByType<BottomBar>();  // Use FindFirstObjectByType for BottomBar
        }
    }

    private void InitializeStats()
    {
        // Set the player class based on the selection in MainMenu
        playerClass = MainMenu.selectedClass;
        
        // Initialize player stats based on the selected class
        switch (playerClass)
        {
            case "Melee":
                maxHealth = 100;
                break;
            case "Ranged":
                maxHealth = 100;
                break;
            case "Magic":
                maxHealth = 100;
                break;
            default:
                maxHealth = 100;  
                break;
        }

    
        health = maxHealth;
    }
}
