using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject map; // The GameObject representing the map.
    [SerializeField] private GameObject enemyPrefab; // The prefab containing the melee enemy hierarchy.
    [SerializeField] private GameObject rangedEnemyPrefab; // The prefab containing the ranged enemy hierarchy.
    [SerializeField] private GameObject bossEnemyPreFab; // The prefab containing the ranged enemy hierarchy.


    public int maxEnemies = 5;
    public int maxBossEnemies = 2;
    public float spawnRadius = 10f;

    private int currentEnemyCount = 0;

    private void Start()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnMeleeEnemyToRandomLocation(2.0f);
            SpawnRangedEnemyToRandomLocation(2.0f);
            SpawnBossEnemyToRandomLocation(2.0f);
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

    private void SpawnRangedEnemyToRandomLocation(float levelMultiplier)
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
        GameObject rangedEnemyInstance = Instantiate(rangedEnemyPrefab, randomLocation, Quaternion.identity);

        // Find the "RangedEnemy" child within the prefab.
        Transform rangedEnemyTransform = rangedEnemyInstance.transform.Find("RangedEnemy");
        if (rangedEnemyTransform == null)
        {
            Debug.LogError("The 'MeleeEnemy' GameObject was not found in the prefab.");
            Destroy(rangedEnemyInstance); // Cleanup the incomplete enemy.
            return;
        }

        // Get the EnemyStats component from the "MeleeEnemy" GameObject.
        EnemyStats enemyStats = rangedEnemyTransform.GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("The 'RangedEnemy' GameObject is missing the EnemyStats component.");
            Destroy(rangedEnemyInstance); // Cleanup the incomplete enemy.
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
    private void SpawnBossEnemyToRandomLocation(float levelMultiplier)
    {
        if (currentEnemyCount >= maxBossEnemies)
        {
            Debug.LogWarning("Maximum number of bosses reached. Cannot spawn more.");
            return;
        }

        if (map == null || bossEnemyPreFab == null)
        {
            Debug.LogError("Map or bossEnemyPrefab is not assigned.");
            return;
        }

        // Get a random spawn location within the map bounds.
        Vector3 randomLocation = GetRandomSpawnPosition(map);

        // Instantiate the boss enemy prefab.
        GameObject bossEnemyInstance = Instantiate(bossEnemyPreFab, randomLocation, Quaternion.identity);

        // Access the EnemyStats script on the prefab
        EnemyStats enemyStats = bossEnemyInstance.GetComponentInChildren<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("EnemyStats component is missing on the boss prefab.");
            Destroy(bossEnemyInstance); // Cleanup the incomplete enemy.
            return;
        }

        // Set the isBoss flag to true for this enemy
        enemyStats.isBoss = true;

        // Randomly determine if the boss is melee or ranged
        bool isMelee = UnityEngine.Random.value > 0.5f;

        // Debug log for boss type
        Debug.Log($"Spawning {(isMelee ? "melee" : "ranged")} boss.");

        // Assign random stats to the boss based on the level multiplier
        enemyStats.maxHealth = UnityEngine.Random.Range(200.0f * levelMultiplier, 500.0f * levelMultiplier);
        enemyStats.health = enemyStats.maxHealth; // Set current health to max health
        enemyStats.Damage = UnityEngine.Random.Range(30.0f * levelMultiplier, 70.0f * levelMultiplier);
        enemyStats.experienceReward = UnityEngine.Random.Range(200, 500);

        // If it's a melee boss, you can add melee-specific logic (e.g., abilities, stats)
        if (isMelee)
        {
            // Additional melee-specific setup if needed (e.g., melee abilities)
            Debug.Log("Melee Boss spawned with additional melee-specific stats.");
        }
        else
        {
            // Additional ranged-specific setup if needed (e.g., ranged abilities)
            Debug.Log("Ranged Boss spawned with additional ranged-specific stats.");
        }

        Debug.Log($"Spawned {(isMelee ? "melee" : "ranged")} boss with {enemyStats.maxHealth} HP, {enemyStats.Damage} Damage, and {enemyStats.experienceReward} XP reward.");

        // Increase the enemy count for bosses
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
