using UnityEngine;
using TMPro;  // For TextMeshPro functionality

public class DeathMessage : MonoBehaviour
{
    // Array of death quotes to choose from
    private string[] deathQuotes = new string[]
    {
        "The light fades, and the battle is lost... You have fallen.",
        "Your journey ends here, adventurer. Rest in the ashes.",
        "Your heart beats no more. The world moves on without you.",
        "A final breath escapes... the shadows claim you.",
        "The earth trembles, and your soul drifts into the unknown.",
        "You fought valiantly... but all things must end.",
        "Fate has decided, and your time is up. Farewell.",
        "The end is nigh... You've been defeated, but not forgotten.",
        "The battle has been lost. Your soul rests, and the memory of you fades from this land.",
        "Another hero falls, their name whispered by the winds.",
        "The final blow has been struck. Death claims all in the end.",
    };

    // Reference to the TextMeshPro component where the death message will be shown
    public TMP_Text deathMessageTextTMP;          

    void Start()
    {
        // Check if the TMP_Text component is assigned
        if (deathMessageTextTMP == null)
        {
            Debug.LogError("Death message TextMeshPro component is not assigned!");
            return;
        }
        ShowDeathMessage();
    }

    // Method to display a random death message
    public void ShowDeathMessage()
    {
        // Select a random death message
        string randomMessage = deathQuotes[Random.Range(0, deathQuotes.Length)];

        // Display the random message on the TextMeshPro component
        deathMessageTextTMP.text = randomMessage;
    }
}
