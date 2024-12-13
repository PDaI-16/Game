using UnityEngine;
using UnityEngine.Tilemaps;

public class EnterPortal : MonoBehaviour
{
    public Map map; // Reference to the Map script
    private GameObject endScreenPanel; // Reference to the end screen panel

    private void Start()
    {
        // Find the EndScreenCanvas in the scene
        GameObject endScreenCanvas = GameObject.Find("EndScreenCanvas");
        if (endScreenCanvas == null)
        {
            Debug.LogError("EndScreenCanvas not found in the scene.");
            return;
        }

        // Find the EndScreenPanel within the EndScreenCanvas
        endScreenPanel = endScreenCanvas.transform.Find("EndScreenPanel").gameObject;
        if (endScreenPanel == null)
        {
            Debug.LogError("EndScreenPanel not found in the EndScreenCanvas.");
        }
        else
        {
            endScreenPanel.SetActive(false); // Ensure it is initially inactive
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the tag "Player"
        {
            // Check for the presence of a BossEnemy
            BossMovement bossMovement = Object.FindFirstObjectByType<BossMovement>();
            if (bossMovement != null)
            {
                Debug.Log("Cannot enter portal. BossEnemy is still present.");
                return; // Exit the method if a BossEnemy is found
            }

            if (map.currentLevel == 5)
            {
                if (endScreenPanel != null)
                {
                    endScreenPanel.SetActive(true);
                }
                Debug.Log("Game Over! You have defeated the final boss.");
                // Add your game over logic here (e.g., show game over screen, stop the game, etc.)
                return;
            }

            Debug.Log("Player entered portal!");
            Debug.Log($"Current level: {map.currentLevel}");

            // Get a random spawn position in the specified biomes
            (Vector3Int spawnPosition, string biomeName) = map.GetRandomSpawnPosition();

            // Print out the biome name
            Debug.Log($"Player spawned in biome: {biomeName}");

            // Move the player to the new position
            other.transform.position = map.tilemap.CellToWorld(spawnPosition);

            // Load the next level
            map.LoadNextLevel();

            // Print out the current tile the player spawned on
            TileBase currentTile = map.tilemap.GetTile(spawnPosition);
            Debug.Log($"Player spawned on tile: {currentTile?.name ?? "None"}");
        }
    }
}