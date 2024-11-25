using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject map; // The GameObject containing spawn locations.

    [SerializeField] private Sprite[] meleeHatSprites;
    [SerializeField] private Sprite[] rangedHatSprites;
    [SerializeField] private Sprite[] magicHatSprites;
    [SerializeField] private GameObject hatPrefab;

    [SerializeField] private Sprite[] meleeWeaponSprites;
    [SerializeField] private Sprite[] rangedWeaponSprites;
    [SerializeField] private Sprite[] magicWeaponSprites;
    [SerializeField] private GameObject weaponPrefab;

    // Called before the first frame update.
    private void Start()
    {
        try
        {
            SpawnRandomHatToRandomLocation(2.0f);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize spawner: {e.Message}");
        }
    }

    // Spawns a random hat at a random location.
    public void SpawnRandomHatToRandomLocation(float levelMultiplier)
    {
        if (map == null)
        {
            Debug.LogError("Map reference is null. Cannot spawn hats.");
            return;
        }

        try
        {
            Hat newHat = GetRandomHat(levelMultiplier);
            /*            Vector3 randomLocation = GetRandomSpawnPosition(map);*/
            Vector3 randomLocation = GetRandomSpawnPosition(map);
            SpawnHat(newHat, randomLocation);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error while spawning hat: {e.Message}");
        }
    }

    // Instantiates a hat at the given location.
    public void SpawnHat(Hat hat, Vector3 location)
    {
        if (hatPrefab == null)
        {
            Debug.LogError("Hat prefab is not assigned.");
            return;
        }

        var hatInstance = Instantiate(hatPrefab, location, Quaternion.identity);

        if (hatInstance.TryGetComponent(out HatGO hatScript))
        {
            hatScript.Initialize(hat);
        }
        else
        {
            Debug.LogError("The instantiated HatPrefab is missing the HatGO component.");
        }
    }

    // Returns a randomly generated hat based on the level multiplier.
    public Hat GetRandomHat(float levelMultiplier)
    {
        if (levelMultiplier <= 0)
        {
            Debug.LogError("Invalid level multiplier. Must be greater than 0.");
            throw new ArgumentException("Level multiplier must be greater than 0.");
        }

        var category = GetRandomEnumValue<ItemCategory>();

        var spriteList = category switch
        {
            ItemCategory.Melee => meleeHatSprites,
            ItemCategory.Ranged => rangedHatSprites,
            ItemCategory.Magic => magicHatSprites,
            _ => throw new ArgumentOutOfRangeException(nameof(category), "Invalid item category.")
        };

        if (spriteList == null || spriteList.Length == 0)
        {
            Debug.LogError($"Sprite list for category {category} is null or empty.");
            throw new InvalidOperationException($"Sprite list for {category} cannot be null or empty.");
        }

        Sprite randomSprite;
        do
        {
            randomSprite = spriteList[UnityEngine.Random.Range(0, spriteList.Length)];
        } while (randomSprite == null);

        var damageMultiplier = UnityEngine.Random.Range(1.0f * levelMultiplier, 3.0f * levelMultiplier);
        var attackSpeedMultiplier = UnityEngine.Random.Range(1.0f * levelMultiplier, 3.0f * levelMultiplier);

        return new Hat(category, randomSprite, damageMultiplier, attackSpeedMultiplier);
    }

    // Returns a random spawn position within the bounds of the given GameObject.
    private Vector3 GetRandomSpawnPosition(GameObject targetMap)
    {
        if (targetMap == null)
        {
            Debug.LogError("Target map is null.");
            return Vector3.zero;
        }

        var renderer = targetMap.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("Renderer not found on the target map.");
            return Vector3.zero;
        }

        var randomX = UnityEngine.Random.Range(renderer.bounds.min.x, renderer.bounds.max.x);
        var randomY = UnityEngine.Random.Range(renderer.bounds.min.y, renderer.bounds.max.y);
        return new Vector3(randomX, randomY, 0);
    }

    // Returns a random value from an Enum type.
    private static T GetRandomEnumValue<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }
}
