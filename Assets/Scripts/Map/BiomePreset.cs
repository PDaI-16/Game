using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Biome Preset", menuName = "New Biome Preset")]
public class BiomePreset : ScriptableObject
{
    public string biomeName;
    public RuleTile ruleTile; // Use RuleTile for biome-specific tile rules
    public float minHeight;
    public float minMoisture;
    public float minHeat;

    [System.Serializable]
    public struct BiomeObject{
        public GameObject prefab;
        [Range(0, 1)] public float spawnProbability;
        public int minDistanceFromEdge; 
    }
    public BiomeObject[] environmentObjects;

    // Method to check if a position matches the biome conditions
    public bool MatchCondition(float height, float moisture, float heat)
    {
        return height >= minHeight && moisture >= minMoisture && heat >= minHeat;
    }
}
