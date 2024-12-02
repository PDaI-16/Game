using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public int maxEnemies = 5;
   public float spawnRadius = 10f;

    private int currentEnemyCount = 0;

    void Start()
    {
        GenerateEnemies();
    }

    void GenerateEnemies()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Random position around the player
        Vector3 spawnPosition = player.position + Random.insideUnitSphere * spawnRadius;

        // Adjust y position for ground level (if needed)
        spawnPosition.y = 0;

        // Instantiate the enemy and increment the counter
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;
    }
}
