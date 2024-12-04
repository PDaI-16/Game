using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject map; // The GameObject representing the map.
    [SerializeField] private GameObject enemyPrefab; // The prefab containing the enemy hierarchy.

    public int maxEnemies = 5;
    public float spawnRadius = 10f;

    private int currentEnemyCount = 0;

    private void Start()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnMeleeEnemyToRandomLocation(2.0f);
        }
    }

    private void SpawnMeleeEnemyToRandomLocation(float levelMultiplier)
    {
        if (currentEnemyCount >= maxEnemies)
        {
            Debug.LogWarning("Maximum number of enemies reached. Cannot spawn more.");
            return;
        }

        if (map == null || enemyPrefab == null)
        {
            Debug.LogError("Map or enemyPrefab is not assigned.");
            return;
        }

        // Get a random spawn location within the map bounds.
        Vector3 randomLocation = GetRandomSpawnPosition(map);

        // Instantiate the enemy prefab at the random location.
        GameObject enemyInstance = Instantiate(enemyPrefab, randomLocation, Quaternion.identity);

        // Find the "MeleeEnemy" child within the prefab.
        Transform meleeEnemyTransform = enemyInstance.transform.Find("MeleeEnemy");
        if (meleeEnemyTransform == null)
        {
            Debug.LogError("The 'MeleeEnemy' GameObject was not found in the prefab.");
            Destroy(enemyInstance); // Cleanup the incomplete enemy.
            return;
        }

        // Get the EnemyStats component from the "MeleeEnemy" GameObject.
        EnemyStats enemyStats = meleeEnemyTransform.GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("The 'MeleeEnemy' GameObject is missing the EnemyStats component.");
            Destroy(enemyInstance); // Cleanup the incomplete enemy.
            return;
        }

        // Assign random stats based on the level multiplier.
        enemyStats.maxHealth = UnityEngine.Random.Range(50.0f * levelMultiplier, 150.0f * levelMultiplier);
        enemyStats.health = enemyStats.maxHealth; // Set current health to max health.
        enemyStats.Damage = UnityEngine.Random.Range(10.0f * levelMultiplier, 30.0f * levelMultiplier);
        enemyStats.experienceReward = UnityEngine.Random.Range(10, 50);

        Debug.Log($"Spawned enemy with {enemyStats.health} HP, {enemyStats.Damage} Damage, and {enemyStats.experienceReward} XP reward.");

        currentEnemyCount++;
    }

    private Vector3 GetRandomSpawnPosition(GameObject targetMap)
    {
        var renderer = targetMap.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("Renderer not found on the map object. Cannot determine spawn bounds.");
            return Vector3.zero;
        }

        float randomX = UnityEngine.Random.Range(renderer.bounds.min.x, renderer.bounds.max.x);
        float randomY = UnityEngine.Random.Range(renderer.bounds.min.y, renderer.bounds.max.y);

        return new Vector3(randomX, randomY, 0);
    }
}
