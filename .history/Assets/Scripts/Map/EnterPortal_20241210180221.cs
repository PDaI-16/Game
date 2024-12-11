using UnityEngine;

public class EnterPortal : MonoBehaviour
{
    public Map map; // Reference to the Map script

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the tag "Player"
        {
            Debug.Log("Player entered portal!");
            Debug.Log($"Current level: {map.currentLevel}");
            map.LoadNextLevel();
        }
    }
}
