using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Tilemaps;
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

    [SerializeField] private int itemSpawnAmount = 50;

    //Damage

    private float minDamageWeapon = 3.0f;
    private float maxDamageWeapon = 10.0f;

    private float minDamageHat = 3.0f;
    private float maxDamageHat = 10.0f;

    //Attackspeeds

    private float minAttackSpeedWeapon = 0.5f;
    private float maxAttackSpeedWeapon = 1.5f;

    private float minAttackSpeedHat = 0.5f;
    private float maxAttackSpeedHat = 1.5f;


    // How steep is the rarity curve ->>> bigger = better items more rare
    private float itemRandomizationSkewFactor = 3.0f;

    // Buffing multiplier to melee weapons
    private float meleeStatsBuffMultiplier = 1.65f;


    private float currentMaxWeaponScore = 0;
    private float currentMaxHatScore = 0;




    // Called before the first frame update.
    public void Start()
    {
        try
        {
            for (int i = 0; i < itemSpawnAmount; i++)
            {
                SpawnRandomHatToRandomLocation(1.0f);
                SpawnRandomWeaponToRandomLocation(1.0f);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize spawner: {e.Message}");
        }
    }

    private void Update()
    {
        
    }

    // This for rarity coloring for items on ground
    public float GetCurrentMaxScoreForItem (ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                return currentMaxWeaponScore;

            case ItemType.Hat:
                return currentMaxHatScore;
        }
        return 0;
    }

    public void DestroyAllItemsOnGround()
    {
        // Find all the MeleeEnemy objects in the scene
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        GameObject[] hats = GameObject.FindGameObjectsWithTag("Hat");

        // Loop through each MeleeEnemy object and destroy it
        foreach (GameObject weapon in weapons)
        {
            if (weapon.GetComponent<WeaponGO>().onGround)
            {
                Destroy(weapon);
            }
        }
        foreach (GameObject hat in hats)
        {
            if (hat.GetComponent<HatGO>().onGround)
            {
                Destroy(hat);
            }
        }


    }

    public Color GetRarityColor(ItemType itemType, float itemScore)
    {
        float scoreRatio = itemType switch
        {
            ItemType.Weapon => itemScore / currentMaxWeaponScore,
            ItemType.Hat => itemScore / currentMaxHatScore,
            _ => 0f // Default for unsupported types
        };

        if (scoreRatio > 0.80f)
            return Color.yellow; // Legendary (Highest Rarity)
        else if (scoreRatio > 0.60f)
            return Color.red; // Epic
        else if (scoreRatio > 0.4f)
            return Color.blue; // Rare
        else if (scoreRatio > 0.2f)
            return Color.green; // Uncommon
        else
            return Color.gray; // Common
    }

    // Spawns a random hat at a random location.
    public void SpawnRandomHatToRandomLocation(float levelMultiplier)
    {
        Tilemap tilemap = map.GetComponent<Tilemap>();

        if (map == null)
        {
            Debug.LogError("Map reference is null. Cannot spawn hats.");
            return;
        }

        try
        {
            Hat newHat = GetRandomHat(levelMultiplier);  // Get a random hat using the level multiplier
            Vector3 randomLocation = GetRandomSpawnPosition(map);  // Get a random spawn position on the map

            
            if (tilemap == null)
            {
                Debug.LogError("Tilemap component not found on the map object.");
                return;
            }


            Vector3Int tilePosition = tilemap.WorldToCell(randomLocation);
            TileBase currentTile = tilemap.GetTile(tilePosition);

            if (currentTile != null)
            {
                Debug.LogWarning(currentTile.name);

                if (currentTile.name == "OceanRule")
                {
                    Debug.LogWarning("THIS IS OCEAN GG");
                }
                else
                {
                    SpawnHat(newHat, map, randomLocation, true);  // Spawn the hat at the random location
                }
            }
           
        }
        catch (Exception e)
        {
            Debug.LogError($"Error while spawning hat: {e.Message}");
        }
    }

    public void SpawnRandomWeaponToRandomLocation(float levelMultiplier)
    {
        Tilemap tilemap = map.GetComponent<Tilemap>();

        if (map == null)
        {
            Debug.LogError("Map reference is null. Cannot spawn weapons.");
            return;
        }

        try
        {
            Weapon newWeapon = GetRandomWeapon(levelMultiplier);  // Get a random weapon using the level multiplier
            Vector3 randomLocation = GetRandomSpawnPosition(map);  // Get a random spawn position on the map


            Vector3Int tilePosition = tilemap.WorldToCell(randomLocation);
            TileBase currentTile = tilemap.GetTile(tilePosition);

            if (currentTile != null)
            {
                Debug.LogWarning(currentTile.name);

                if (currentTile.name == "OceanRule")
                {
                    Debug.LogWarning("ITEM SPAWNED TO OCEAN GG");
                }
                else
                {
                    SpawnWeapon(newWeapon, map, randomLocation, true);  // Spawn the weapon at the random location
                }
            }
            
        }
        catch (Exception e)
        {
            Debug.LogError($"Error while spawning weapon: {e.Message}");
        }
    }

    public GameObject SpawnHat(Hat hat, GameObject parentObject, Vector3 location, bool isOnGround)
    {
        if (hatPrefab == null)
        {
            Debug.LogError("Hat prefab is not assigned.");
            return null;  // Return null if prefab is missing
        }

        if (parentObject == null)
        {
            Debug.LogError("Parent object is not assigned.");
            return null;  // Return null if parent object is missing
        }

        // Instantiate the hat prefab at the given location
        var hatInstance = Instantiate(hatPrefab, location, Quaternion.identity);

        // Set the instantiated hat as a child of the parent object
        hatInstance.transform.SetParent(parentObject.transform);

        // Reset the rotation and scale if necessary
        hatInstance.transform.localRotation = Quaternion.identity;
        hatInstance.transform.localScale = Vector3.one;
        hatInstance.transform.position = location;

        // Try to get the HatGO script on the instantiated hat
        if (hatInstance.TryGetComponent(out HatGO hatScript))
        {
            // Initialize the hat script with the provided hat data and the isOnGround flag
            hatScript.Initialize(hat, isOnGround);
            Debug.Log("Hat spawned and initialized successfully!");
        }
        else
        {
            Debug.LogError("The instantiated HatPrefab is missing the HatGO component.");
        }

        // Return the hat instance so you can use it outside this method
        return hatInstance;
    }

    /// <summary>
    /// Spawns a weapon as a child of the specified parent object at the provided location.
    /// The weapon will be initialized with the provided weapon data.
    /// </summary>
    /// <param name="weapon">The weapon data to initialize the spawned weapon.</param>
    /// <param name="parentObject">The GameObject that will act as the parent for the weapon.</param>
    /// <param name="location">The world-space position where the weapon should spawn, relative to the parent object.</param>
    /// <param name="isOnGround">A flag indicating whether the weapon is on the ground, which makes picking up possible</param>

    public GameObject SpawnWeapon(Weapon weapon, GameObject parentObject, Vector2 location, bool isOnGround)
    {
        if (weaponPrefab == null)
        {
            Debug.LogError("Weapon prefab is not assigned.");
            return null;  // Return null if prefab is missing
        }

        if (parentObject == null)
        {
            Debug.LogError("Parent object is not assigned.");
            return null;  // Return null if parent object is missing
        }

        // Instantiate the weapon prefab at the given location
        var weaponInstance = Instantiate(weaponPrefab, location, Quaternion.identity);

        // Set the instantiated weapon as a child of the parent object
        weaponInstance.transform.SetParent(parentObject.transform);

        // Reset the rotation and scale if necessary
        weaponInstance.transform.localRotation = Quaternion.identity;
        weaponInstance.transform.localScale = Vector3.one;
        weaponInstance.transform.localPosition = location;

        // Try to get the WeaponGO script on the instantiated weapon
        if (weaponInstance.TryGetComponent(out WeaponGO weaponScript))
        {
            // Initialize the weapon script with the provided weapon data
            weaponScript.Initialize(weapon, isOnGround);
        }
        else
        {
            Debug.LogError("The instantiated WeaponPrefab is missing the WeaponGO component.");
        }

        // Return the weapon instance so you can use it outside this method
        return weaponInstance;
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


        currentMaxHatScore = (maxDamageHat * levelMultiplier) * (maxAttackSpeedHat * levelMultiplier);

        var damageMultiplier = GetSkewedRandom(minDamageHat * levelMultiplier, maxDamageHat * levelMultiplier, itemRandomizationSkewFactor); // 2f = skew factor
        var attackSpeedMultiplier = GetSkewedRandom(minAttackSpeedHat * levelMultiplier, maxAttackSpeedHat * levelMultiplier, itemRandomizationSkewFactor);

        return new Hat(category, randomSprite, damageMultiplier, attackSpeedMultiplier, meleeStatsBuffMultiplier);
    }

    public Weapon GetRandomWeapon(float levelMultiplier)
    {
        if (levelMultiplier <= 0)
        {
            Debug.LogError("Invalid level multiplier. Must be greater than 0.");
            throw new ArgumentException("Level multiplier must be greater than 0.");
        }

        var category = GetRandomEnumValue<ItemCategory>();

        // Assuming you have different sprite lists based on category
        var spriteList = category switch
        {
            ItemCategory.Melee => meleeWeaponSprites,  // Replace with your actual sprite list for melee weapons
            ItemCategory.Ranged => rangedWeaponSprites,  // Replace with your actual sprite list for ranged weapons
            ItemCategory.Magic => magicWeaponSprites,  // Replace with your actual sprite list for magic weapons
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

        // Define weapon-specific properties like damage, attack speed, or other stats

        currentMaxWeaponScore = (maxDamageWeapon * levelMultiplier) * (maxAttackSpeedWeapon * levelMultiplier);

        var damage = GetSkewedRandom(minDamageWeapon * levelMultiplier, maxDamageWeapon * levelMultiplier, itemRandomizationSkewFactor); // 2f = skew factor
        var attackSpeed = GetSkewedRandom(minAttackSpeedWeapon * levelMultiplier, maxAttackSpeedWeapon * levelMultiplier,itemRandomizationSkewFactor);

        // Create and return a new Weapon instance
        return new Weapon(category, randomSprite, damage, attackSpeed, meleeStatsBuffMultiplier);
    }

    private float GetSkewedRandom(float min, float max, float skewFactor)
    {

        /*        Range UniformDistribution | Skew Factor = 2
                        [0.0, 0.1]      10 % 19 %

                        [0.1, 0.2]      10 % 17 %

                        [0.2, 0.3]      10 % 15 %

                        [0.3, 0.4]      10 % 13 %

                        [0.4, 0.5]      10 % 11 %

                        [0.5, 0.6]      10 % 9 %

                        [0.6, 0.7]      10 % 7 %

                        [0.7, 0.8]      10 % 5 %

                        [0.8, 0.9]      10 % 3 %

                        [0.9, 1.0]      10 % 1 %*/


        // Generate a random value between 0 and 1 using Unity's Random
        float t = UnityEngine.Random.Range(0f, 1f);

        // Apply skew (e.g., use a power function to skew towards min)
        t = Mathf.Pow(t, skewFactor);

        // Map back to the desired range and clamp the result to avoid exceeding max
        return Mathf.Clamp(min + (max - min) * t, min, max);
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
