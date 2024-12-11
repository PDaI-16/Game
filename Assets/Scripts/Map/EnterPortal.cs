using UnityEngine;

public class EnterPortal : MonoBehaviour
{
    public Map map; // Reference to the Map script
    public BossMovement BossMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the tag "Player"
        {
            // Check for the presence of a BossEnemy
            BossMovement = Object.FindFirstObjectByType<BossMovement>();
            if (BossMovement != null)
            {
                Debug.Log("Cannot enter portal. BossEnemy is still present.");
                return; // Exit the method if a BossEnemy is found
            }

            Debug.Log("Player entered portal!");
            Debug.Log($"Current level: {map.currentLevel}");

            // Get a random spawn position in the specified biomes
            (Vector3Int spawnPosition, string biomeName) = map.GetRandomSpawnPosition();

            // Print out the biome name
            Debug.Log($"Player spawned in biome: {biomeName}");

            // Load the next level
            map.LoadNextLevel();
            // Move the player to the new position
            other.transform.position = map.tilemap.CellToWorld(spawnPosition);

            
        }
    }
}
