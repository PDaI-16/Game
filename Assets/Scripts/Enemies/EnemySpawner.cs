using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject map; // The GameObject representing the map.
    [SerializeField] private GameObject meleeEnemyPrefab; // The prefab containing the melee enemy hierarchy.
    [SerializeField] private GameObject rangedEnemyPrefab; // The prefab containing the ranged enemy hierarchy.
    [SerializeField] private GameObject bossEnemyPreFab; // The prefab containing the ranged enemy hierarchy.


    public int maxEnemies = 25;
    public int maxBossEnemies = 1;
    public int currentBossEnemyCount = 0;
    public int currentEnemyCount = 0;
    public float spawnRadius = 10f;

    private Map maps;

    public void Start()
    {
            SpawnBossEnemyToRandomLocation(2.0f);
            SpawnMeleeEnemyToRandomLocation(2.0f);
            SpawnRangedEnemyToRandomLocation(2.0f);
            
    }

    private void SpawnMeleeEnemyToRandomLocation(float levelMultiplier)
    {
    for (int i = 0; i < 12; i++)
    {
        if (currentEnemyCount >= maxEnemies)
        {
            Debug.LogWarning("Maximum number of enemies reached. Cannot spawn more.");
            return;
        }

        if (map == null || meleeEnemyPrefab == null)
        {
            Debug.LogError("Map or meleeEnemyPrefab is not assigned.");
            return;
        }

        // Get a random spawn location within the map bounds.
        Vector3 randomLocation = GetRandomSpawnPosition(map);

        if (randomLocation == Vector3.zero)
        {
            Debug.LogError("Failed to find a suitable spawn position for a Melee enemy.");
            return;
        }

        // Instantiate the enemy prefab at the random location.
        GameObject meleeEnemyInstance = Instantiate(meleeEnemyPrefab, randomLocation, Quaternion.identity);

        // Find the "MeleeEnemy" child within the prefab.
        Transform meleeEnemyTransform = meleeEnemyInstance.transform.Find("MeleeEnemy");
        if (meleeEnemyTransform == null)
        {
            Debug.LogError("The 'MeleeEnemy' GameObject was not found in the prefab.");
            Destroy(meleeEnemyInstance); // Cleanup the incomplete enemy.
            return;
        }

        // Get the EnemyStats component from the "MeleeEnemy" GameObject.
        EnemyStats enemyStats = meleeEnemyTransform.GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("The 'MeleeEnemy' GameObject is missing the EnemyStats component.");
            Destroy(meleeEnemyInstance); // Cleanup the incomplete enemy.
            return;
        }

        // Assign random stats based on the level multiplier.
        enemyStats.maxHealth = UnityEngine.Random.Range(80.0f * levelMultiplier, 100.0f * levelMultiplier);
        enemyStats.health = enemyStats.maxHealth; // Set current health to max health.
        enemyStats.Damage = UnityEngine.Random.Range(10.0f * levelMultiplier, 30.0f * levelMultiplier);
        enemyStats.experienceReward = UnityEngine.Random.Range(5, 25);

        Tilemap tilemap = map.GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found on the map object.");
            return;
        }
        Vector3Int tilePosition = tilemap.WorldToCell(randomLocation);
        TileBase currentTile = tilemap.GetTile(tilePosition);
        Debug.Log($"Melee enemy spawned on tile: {currentTile?.name ?? "None"}");

        currentEnemyCount++;
    }
}

    public void DestroyAllEnemies()
    {
        // Find all the MeleeEnemy objects in the scene
        GameObject[] meleeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Loop through each MeleeEnemy object and destroy it
        foreach (GameObject enemy in meleeEnemies)
        {
            Destroy(enemy);
        }

        // Reset the current enemy count to 0
        currentEnemyCount = 0;
    }

    private void SpawnRangedEnemyToRandomLocation(float levelMultiplier)
{
    for (int i = 0; i < 12; i++)
    {
        if (currentEnemyCount >= maxEnemies)
        {
            Debug.LogWarning("Maximum number of enemies reached. Cannot spawn more.");
            return;
        }

        if (map == null || rangedEnemyPrefab == null)
        {
            Debug.LogError("Map or rangedEnemyPrefab is not assigned.");
            return;
        }

        // Get a random spawn location within the map bounds.
        Vector3 randomLocation = GetRandomSpawnPosition(map);

        if (randomLocation == Vector3.zero)
        {
            Debug.LogError("Failed to find a suitable spawn position for a Ranged enemy.");
            return;
        }

        // Instantiate the enemy prefab at the random location.
        GameObject rangedEnemyInstance = Instantiate(rangedEnemyPrefab, randomLocation, Quaternion.identity);

        // Access the EnemyStats script on the prefab
        EnemyStats enemyStats = rangedEnemyInstance.GetComponentInChildren<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("EnemyStats component is missing on the ranged enemy prefab.");
            Destroy(rangedEnemyInstance); // Cleanup the incomplete enemy.
            return;
        }

        // Assign random stats based on the level multiplier.
        enemyStats.maxHealth = UnityEngine.Random.Range(80.0f * levelMultiplier, 110.0f * levelMultiplier);
        enemyStats.health = enemyStats.maxHealth; // Set current health to max health.
        enemyStats.Damage = UnityEngine.Random.Range(10.0f * levelMultiplier, 30.0f * levelMultiplier);
        enemyStats.experienceReward = UnityEngine.Random.Range(5, 25);

        Tilemap tilemap = map.GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found on the map object.");
            return;
        }
        Vector3Int tilePosition = tilemap.WorldToCell(randomLocation);
        TileBase currentTile = tilemap.GetTile(tilePosition);
        Debug.Log($"Ranged enemy spawned on tile: {currentTile?.name ?? "None"}");

        currentEnemyCount++;
    }
}

    public void  SpawnBossEnemyToRandomLocation(float levelMultiplier)
    {
        if (currentBossEnemyCount >= maxBossEnemies)
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

        if (randomLocation == Vector3.zero)
        {
            Debug.LogError("Failed to find a suitable spawn position for the boss enemy.");
            return;
        }
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

        // Assign random stats to the boss based on the level multiplier
        enemyStats.maxHealth = UnityEngine.Random.Range(500.0f * levelMultiplier, 850.0f * levelMultiplier);
        enemyStats.health = enemyStats.maxHealth; // Set current health to max health
        enemyStats.Damage = UnityEngine.Random.Range(15.0f * levelMultiplier, 30.0f * levelMultiplier);
        enemyStats.experienceReward = UnityEngine.Random.Range(100, 250);

        Tilemap tilemap = map.GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found on the map object.");
            return;
        }
        Vector3Int tilePosition = tilemap.WorldToCell(randomLocation);
        TileBase currentTile = tilemap.GetTile(tilePosition);
        Debug.Log($"Boss spawned on tile: {currentTile?.name ?? "None"}");
        // Increase the enemy count for bosses
        currentBossEnemyCount++;
    }

private Vector3 GetRandomSpawnPosition(GameObject targetMap)
{
    if (map == null)
    {
        Debug.LogError("Map reference is not assigned.");
        return Vector3.zero;
    }

    var renderer = targetMap.GetComponent<Renderer>();
    if (renderer == null)
    {
        Debug.LogError("Renderer not found on the map object. Cannot determine spawn bounds.");
        return Vector3.zero;
    }

    Vector3 randomPosition;
    TileBase currentTile;
    int maxAttempts = 100; // Limit the number of attempts to prevent an infinite loop
    int attempts = 0;

    do
    {
        float randomX = UnityEngine.Random.Range(renderer.bounds.min.x, renderer.bounds.max.x);
        float randomY = UnityEngine.Random.Range(renderer.bounds.min.y, renderer.bounds.max.y);
        randomPosition = new Vector3(randomX, randomY, 0);

        Tilemap tilemap = map.GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found on the map object.");
            return Vector3.zero;
        }
        Vector3Int tilePosition = tilemap.WorldToCell(randomPosition);
        currentTile = tilemap.GetTile(tilePosition);

        attempts++;
    } while (currentTile != null && currentTile.name == "OceanRule" && attempts < maxAttempts);

    if (attempts >= maxAttempts)
    {
        Debug.LogWarning("Failed to find a suitable spawn position after maximum attempts.");
        return Vector3.zero; // Return zero vector if no suitable positions found
    }

    return randomPosition;
}
}