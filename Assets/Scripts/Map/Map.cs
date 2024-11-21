using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Map : MonoBehaviour
{
    public BiomePreset[] biomes;
    public Tilemap tilemap;  // Reference to the Tilemap component
    public GameObject colliderParent;

    [Header("Dimensions")]
    public int width = 50;
    public int height = 50;
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
    }

    void GenerateMap()
    {
        // Generate noise maps for height, moisture, and heat
        heightMap = NoiseGenerator.GenerateNoiseMap(width, height, scale, offset, heightWaves);
        moistureMap = NoiseGenerator.GenerateNoiseMap(width, height, scale, offset, moistureWaves);
        heatMap = NoiseGenerator.GenerateNoiseMap(width, height, scale, offset, heatWaves);

        // Iterate over each tile position
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                // Determine the biome of the current tile
                BiomePreset currentBiome = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]);

                // Set the tile to the biome's RuleTile
                if (currentBiome != null && currentBiome.ruleTile != null)
                {
                    tilemap.SetTile(position, currentBiome.ruleTile);
                }
            }
        }
        GenerateEnvironmentObjects();
        GenerateOceanColliders();
        
    }

   void GenerateEnvironmentObjects()
{
    int minDistanceFromMapEdge = 2; // Minimum distance from the edge of the map
    List<Bounds> occupiedBounds = new List<Bounds>();

    for (int x = 0; x < width; ++x)
    {
        for (int y = 0; y < height; ++y)
        {
            Vector3Int position = new Vector3Int(x, y, 0);
            BiomePreset currentBiome = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]);

            if (currentBiome == null)
                continue;

            foreach (var biomeObject in currentBiome.environmentObjects)
            {
                if (Random.value < biomeObject.spawnProbability)
                {
                    // Skip spawning if too close to the map edge
                    if (x < minDistanceFromMapEdge || x >= width - minDistanceFromMapEdge ||
                        y < minDistanceFromMapEdge || y >= height - minDistanceFromMapEdge)
                    {
                        continue;
                    }

                    // Calculate the distance from the biome edge
                    int distanceFromBiomeEdge = CalculateDistanceFromBiomeEdge(x, y, currentBiome);

                    // Check if the distance from the edge meets the requirement for this object
                    if (distanceFromBiomeEdge < biomeObject.minDistanceFromEdge)
                    {
                        continue; // Skip spawning if too close to the biome edge
                    }

                    // Calculate spawn position with a random offset
                    Vector3 spawnPosition = new Vector3(x + Random.Range(-0.5f, 0.5f), y + Random.Range(-0.5f, 0.5f), 0);

                    // Calculate the bounding box of the prefab considering its scale
                    Renderer renderer = biomeObject.prefab.GetComponent<Renderer>();
                    if (renderer == null)
                    {
                        Debug.LogWarning("Prefab does not have a Renderer component: " + biomeObject.prefab.name);
                        continue;
                    }
                    Bounds newObjectBounds = renderer.bounds;
                    newObjectBounds.center = spawnPosition;

                    // Check if the bounding box is fully contained within the intended biome
                    bool isFullyContained = true;
                    for (float bx = newObjectBounds.min.x; bx <= newObjectBounds.max.x; bx += 1.0f)
                    {
                        for (float by = newObjectBounds.min.y; by <= newObjectBounds.max.y; by += 1.0f)
                        {
                            int checkX = Mathf.FloorToInt(bx);
                            int checkY = Mathf.FloorToInt(by);

                            if (checkX < 0 || checkY < 0 || checkX >= width || checkY >= height)
                            {
                                isFullyContained = false;
                                break;
                            }

                            BiomePreset nearbyBiome = GetBiome(heightMap[checkX, checkY], moistureMap[checkX, checkY], heatMap[checkX, checkY]);
                            if (nearbyBiome != currentBiome)
                            {
                                isFullyContained = false;
                                break;
                            }
                        }
                        if (!isFullyContained) break;
                    }

                    if (!isFullyContained)
                    {
                        continue; // Skip spawning if the bounding box is not fully contained within the intended biome
                    }

                    // Check if the position is already occupied by considering the object's size
                    bool overlaps = false;
                    foreach (var bounds in occupiedBounds)
                    {
                        if (bounds.Intersects(newObjectBounds))
                        {
                            overlaps = true;
                            break;
                        }
                    }

                    if (overlaps)
                    {
                        continue; // Skip spawning if the position is already occupied
                    }

                    // Instantiate the object and mark the position as occupied
                    Instantiate(biomeObject.prefab, spawnPosition, Quaternion.identity, transform);
                    occupiedBounds.Add(newObjectBounds);
                    //Debug.Log("Spawned " + biomeObject.prefab.name + " at " + spawnPosition);
                }
            }
        }
    }
}

void GenerateOceanColliders()
{
    // Check if the OceanColliders GameObject already exists and remove it
    Transform existingOceanColliders = transform.Find("OceanColliders");
    if (existingOceanColliders != null)
    {
        DestroyImmediate(existingOceanColliders.gameObject);
    }

    // Create a new GameObject for ocean colliders
    GameObject oceanCollidersParent = new GameObject("OceanColliders");
    oceanCollidersParent.transform.parent = transform;

    // Reference the Tilemap's origin position
    Vector3 tilemapOrigin = tilemap.origin;

    bool[,] visited = new bool[width, height];

    for (int x = 0; x < width; ++x)
    {
        for (int y = 0; y < height; ++y)
        {
            if (visited[x, y])
                continue;

            BiomePreset currentBiome = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]);

            // Check if the current tile belongs to the Ocean biome
            if (currentBiome != null && currentBiome.name == "Ocean")
            {
                // Perform flood fill to determine the size of the contiguous ocean area
                List<Vector2Int> oceanArea = new List<Vector2Int>();
                FloodFillOcean(x, y, oceanArea, visited);

                // Determine the width and height of the ocean area
                int minX = oceanArea.Min(pos => pos.x);
                int maxX = oceanArea.Max(pos => pos.x);
                int minY = oceanArea.Min(pos => pos.y);
                int maxY = oceanArea.Max(pos => pos.y);

                int areaWidth = maxX - minX + 1;
                int areaHeight = maxY - minY + 1;

                // Skip generating colliders if the area is too small
                if (areaWidth < 3 || areaHeight < 3)
                    continue;

                // Generate colliders for the ocean area
                foreach (var pos in oceanArea)
                {
                    Vector3 tilePosition = tilemap.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0));

                    BoxCollider2D oceanCollider = oceanCollidersParent.AddComponent<BoxCollider2D>();
                    oceanCollider.offset = tilePosition - transform.position; // Align collider to the tile center
                    oceanCollider.size = Vector2.one; // Assuming each tile is 1x1 in size
                    oceanCollider.isTrigger = false; // Ensure the collider is not a trigger
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

    Debug.Log($"Distance from biome edge for ({x}, {y}): {distance}");
    return distance;
}


     BiomePreset GetBiome(float height, float moisture, float heat)
    {
        // Loop through each biome and return the first one that matches the conditions
        foreach (BiomePreset biome in biomes)
        {
            if (biome.MatchCondition(height, moisture, heat))
            {
                return biome;
            }
        }

        // If no biome matches, you could return a default biome or null
        return null;
    }
}
