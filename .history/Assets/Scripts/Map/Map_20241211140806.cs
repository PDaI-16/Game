using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Math = System.Math;

public class Map : MonoBehaviour
{
    public BiomePreset[] biomes;
    public Tilemap tilemap;  // Reference to the Tilemap component
    public GameObject colliderParent;
    public RuleTile borderTile; // Assign this in the Inspector
    public Tilemap BorderTilemap;

    public int currentLevel = 1;  // Start with Level 1
    public int maxLevels = 5;     // Total levels to generate

    public bool Randomized = false;

    [Header("Prefabs")]
    public GameObject portalPrefab; // Add this field for the portal prefab

    [Header("Dimensions")]
    public int width = 60;
    public int height = 60;
    public float scale = 1.0f;
    public Vector2 offset;

    [Header("Height Map")]
    public Wave[] heightWaves;
    private float[,] heightMap;

    [Header("Moisture Map")]
    public Wave[] moistureWaves;
    private float[,] moistureMap;

    [Header("Heat Map")]
    public Wave[] heatWaves;
    private float[,] heatMap;

    void Start()
    {
        GenerateMap();
        GenerateEnvironmentObjects();
        GenerateOceanColliders();
    }

void GenerateMap()
{
    if (Randomized)
    {
        System.Random random = new System.Random();
        heightWaves[0].seed = (float)Math.Round(random.NextDouble() * 1000, 2);
        heightWaves[1].seed = (float)Math.Round(random.NextDouble() * 1000, 2);
        moistureWaves[0].seed = (float)Math.Round(random.NextDouble() * 1000, 2);
        heatWaves[0].seed = (float)Math.Round(random.NextDouble() * 1000, 2);
        heatWaves[1].seed = (float)Math.Round(random.NextDouble() * 1000, 2);
    }
    else
    {
        heightWaves[0].seed = 56f;
        heightWaves[1].seed = 199.36f;
        moistureWaves[0].seed = 621f;
        heatWaves[0].seed = 318.6f;
        heatWaves[1].seed = 329.7f;
    }

    // Generate noise maps for height, moisture, and heat
    heightMap = NoiseGenerator.GenerateNoiseMap(width, height, scale, offset, heightWaves);
    moistureMap = NoiseGenerator.GenerateNoiseMap(width, height, scale, offset, moistureWaves);
    heatMap = NoiseGenerator.GenerateNoiseMap(width, height, scale, offset, heatWaves);

    // Generate borders only for the first level
    if (currentLevel == 1)
    {
        int borderSize = 26;

        for (int x = -1; x <= borderSize; ++x) // Extend bounds for borders
        {
            for (int y = -1; y <= borderSize; ++y) // Extend bounds for borders
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                if (IsBorder(x, y))
                {
                    BorderTilemap.SetTile(position, borderTile); // Ensure borderTile is assigned
                }
            }
        }
    }

    // Iterate over each tile position for the main map
    for (int x = 0; x < width; ++x)
    {
        for (int y = 0; y < height; ++y)
        {
            Vector3Int position = new Vector3Int(x, y, 0);

            // Place regular tiles only within playable bounds
            BiomePreset currentBiome = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]);

            if (currentBiome != null && currentBiome.ruleTile != null)
            {
                tilemap.SetTile(position, currentBiome.ruleTile);
            }
        }
    }
}

public void LoadNextLevel()
{
    // Clear old map and colliders
    ResetMap();

    // Enable randomization for the next level
    Randomized = true;
    currentLevel++;

    // Generate a new map
    GenerateMap();

    // Regenerate environment objects
    GenerateEnvironmentObjects();

    // Regenerate ocean colliders
    GenerateOceanColliders();

    Debug.Log($"Next level generated! {currentLevel}");
}


void ResetMap()
{
    // Destroy any leftover colliders
    Transform existingOceanColliders = transform.Find("OceanColliders");
    if (existingOceanColliders != null)
    {
        Destroy(existingOceanColliders.gameObject);
    }

    // Clear tiles in the main tilemap
    tilemap.ClearAllTiles();

    // Destroy any leftover colliders
    foreach (Transform child in colliderParent.transform)
    {
        // Check if the child is not part of the BorderTilemap
        if (child.gameObject != BorderTilemap.gameObject)
        {
            Destroy(child.gameObject);
        }
    }
}

bool IsBorder(int x, int y)
{
    // Fixed border size of 45x45
    int borderSize = 26;
    return x < 0 || y < 0 || x >= borderSize || y >= borderSize;
}

void GenerateEnvironmentObjects()
{
    int minDistanceFromMapEdge = 2; // Minimum distance from the edge of the map
    List<Bounds> occupiedBounds = new List<Bounds>();
    bool portalSpawned = false; // Flag to check if the portal has been spawned

    // Create a list of all possible positions
    List<Vector2Int> positions = new List<Vector2Int>();
    for (int x = 0; x < width; ++x)
    {
        for (int y = 0; y < height; ++y)
        {
            positions.Add(new Vector2Int(x, y));
        }
    }

    // Shuffle the list of positions
    System.Random rng = new System.Random();
    int n = positions.Count;
    while (n > 1)
    {
        n--;
        int k = rng.Next(n + 1);
        Vector2Int value = positions[k];
        positions[k] = positions[n];
        positions[n] = value;
    }

    // Iterate through the shuffled list to find a suitable position for the portal
    foreach (Vector2Int pos in positions)
    {
        int x = pos.x;
        int y = pos.y;
        Vector3Int position = new Vector3Int(x, y, 0);
        BiomePreset currentBiome = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]);

        if (currentBiome == null)
            continue;

        // Check if the current biome is suitable for spawning the portal
        bool isSuitableForPortal = currentBiome.name == "Desert" || currentBiome.name == "Forest" || currentBiome.name == "Grassland";

        // Spawn the portal if it hasn't been spawned yet, the current biome is suitable, and the position is not too close to the edge
        if (!portalSpawned && isSuitableForPortal && Random.value < 0.01f &&
            x >= minDistanceFromMapEdge && x < width - minDistanceFromMapEdge &&
            y >= minDistanceFromMapEdge && y < height - minDistanceFromMapEdge) // Adjust the probability as needed
        {
            GameObject portal = Instantiate(portalPrefab, tilemap.CellToWorld(position), Quaternion.identity, colliderParent.transform);
            EnterPortal portalScript = portal.GetComponent<EnterPortal>();
            if (portalScript != null)
            {
                portalScript.map = this; // Set the reference to the Map component
            }
            portalSpawned = true;
        }

        foreach (var biomeObject in currentBiome.environmentObjects)
        {
            if (Random.value < biomeObject.spawnProbability)
            {
                if (x < minDistanceFromMapEdge || x >= width - minDistanceFromMapEdge ||
                    y < minDistanceFromMapEdge || y >= height - minDistanceFromMapEdge)
                {
                    // Skip objects too close to the edge
                    continue;
                }

                // Calculate distance from biome edge
                int distanceFromBiomeEdge = CalculateDistanceFromBiomeEdge(x, y, currentBiome);
                if (distanceFromBiomeEdge < biomeObject.minDistanceFromEdge)
                {
                    // Skip objects too close to the biome edge
                    continue;
                }

                // Instantiate the environment object
                GameObject obj = Instantiate(biomeObject.prefab, tilemap.CellToWorld(position), Quaternion.identity, colliderParent.transform);
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer == null)
                {
                    Destroy(obj);
                    continue;
                }

                Bounds bounds = renderer.bounds;
                bool isFullyContained = true;

                // Calculate the corners of the bounds
                Vector3[] corners = new Vector3[4];
                corners[0] = bounds.min;
                corners[1] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
                corners[2] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
                corners[3] = bounds.max;

                foreach (Vector3 corner in corners)
                {
                    Vector3Int cellPosition = tilemap.WorldToCell(corner);
                    if (cellPosition.x < 0 || cellPosition.y < 0 || cellPosition.x >= width || cellPosition.y >= height)
                    {
                        isFullyContained = false;
                        break;
                    }

                    BiomePreset nearbyBiome = GetBiome(heightMap[cellPosition.x, cellPosition.y], moistureMap[cellPosition.x, cellPosition.y], heatMap[cellPosition.x, cellPosition.y]);
                    if (nearbyBiome != currentBiome)
                    {
                        isFullyContained = false;
                        break;
                    }
                }

                if (!isFullyContained)
                {
                    Destroy(obj);
                    continue; // Skip spawning if the bounding box is not fully contained within the intended biome
                }

                // Check if the position is already occupied by considering the object's size
                bool overlaps = false;
                foreach (var occupied in occupiedBounds)
                {
                    if (bounds.Intersects(occupied))
                    {
                        overlaps = true;
                        break;
                    }
                }

                if (overlaps)
                {
                    Destroy(obj);
                    continue; // Skip spawning if the position is already occupied
                }

                // Mark the position as occupied
                occupiedBounds.Add(bounds);
            }
        }
    }
}

void GenerateOceanColliders()
{
    Transform existingOceanColliders = transform.Find("OceanColliders");
    if (existingOceanColliders != null)
    {
        Destroy(existingOceanColliders.gameObject);
    }

    GameObject oceanCollidersParent = new GameObject("OceanColliders");
    oceanCollidersParent.transform.parent = transform;

    bool[,] visited = new bool[width, height];

    for (int x = 0; x < width; ++x)
    {
        for (int y = 0; y < height; ++y)
        {
            if (visited[x, y])
                continue;

            BiomePreset currentBiome = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]);

            if (currentBiome != null && currentBiome.name == "Ocean")
            {
                List<Vector2Int> oceanArea = new List<Vector2Int>();
                FloodFillOcean(x, y, oceanArea, visited);

                // Skip borders when generating colliders
               

                foreach (var pos in oceanArea)
                {
                    Vector3 tilePosition = tilemap.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0));
                    BoxCollider2D oceanCollider = oceanCollidersParent.AddComponent<BoxCollider2D>();
                    oceanCollider.offset = tilePosition - transform.position;
                    oceanCollider.size = Vector2.one;
                }
            }
        }
    }
}

void FloodFillOcean(int startX, int startY, List<Vector2Int> oceanArea, bool[,] visited)
{
    Queue<Vector2Int> queue = new Queue<Vector2Int>();
    queue.Enqueue(new Vector2Int(startX, startY));
    visited[startX, startY] = true;

    while (queue.Count > 0)
    {
        Vector2Int current = queue.Dequeue();
        oceanArea.Add(current);

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (Mathf.Abs(dx) == Mathf.Abs(dy))
                    continue; // Skip diagonals

                int newX = current.x + dx;
                int newY = current.y + dy;

                if (newX >= 0 && newY >= 0 && newX < width && newY < height && !visited[newX, newY])
                {
                    BiomePreset nearbyBiome = GetBiome(heightMap[newX, newY], moistureMap[newX, newY], heatMap[newX, newY]);
                    if (nearbyBiome != null && nearbyBiome.name == "Ocean")
                    {
                        queue.Enqueue(new Vector2Int(newX, newY));
                        visited[newX, newY] = true;
                    }
                }
            }
        }
    }
}



int CalculateDistanceFromBiomeEdge(int x, int y, BiomePreset biome)
{
    int distance = 0;
    bool isEdge = false;

    while (!isEdge && distance < Mathf.Max(width, height))
    {
        for (int dx = -distance; dx <= distance; dx++)
        {
            for (int dy = -distance; dy <= distance; dy++)
            {
                int checkX = x + dx;
                int checkY = y + dy;

                if (checkX < 0 || checkY < 0 || checkX >= width || checkY >= height)
                    continue;

                BiomePreset nearbyBiome = GetBiome(heightMap[checkX, checkY], moistureMap[checkX, checkY], heatMap[checkX, checkY]);
                if (nearbyBiome != biome)
                {
                    isEdge = true;
                    break;
                }
            }
            if (isEdge) break;
        }
        distance++;
    }

    return distance;
}


    BiomePreset GetBiome(float height, float moisture, float heat)
{
    // Define thresholds or weights for each biome
    float desertThreshold = 0.7f; // Example threshold for desert biome
    float forestThreshold = 0.5f; // Example threshold for forest biome
    float grasslandThreshold = 0.4f; // Example threshold for grassland biome
    float mountainThreshold = 0.8f; // Example threshold for mountain biome
    float oceanThreshold = 0.45f; // Example threshold for ocean biome


    foreach (BiomePreset biome in biomes)
    {
        if (biome.name == "Desert" && height > desertThreshold)
        {
            return biome;
        }
        else if (biome.name == "Forest" && height > forestThreshold)
        {
            return biome;
        }
        else if (biome.name == "Grassland" && height > grasslandThreshold)
        {
            return biome;
        }
        else if (biome.name == "Mountains" && height > mountainThreshold)
        {
            return biome;
        }
        else if (biome.name == "Ocean" && height < oceanThreshold)
        {
            return biome;
        }
        else if (biome.MatchCondition(height, moisture, heat))
        {
            return biome;
        }
    }

    // If no biome matches, you could return a default biome or null
    return null;
}
}
