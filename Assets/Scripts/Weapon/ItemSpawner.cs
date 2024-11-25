using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject Map; // The gameobject where we get the possible locations to spawn

    [SerializeField] private Sprite[] MeleeHatSprites;
    [SerializeField] private Sprite[] RangedHatSprites;
    [SerializeField] private Sprite[] MagicHatSprites;
    [SerializeField] private GameObject HatPrefab;

    [SerializeField] private Sprite[] MeleeWeaponSprites;
    [SerializeField] private Sprite[] RangedWeaponSprites;
    [SerializeField] private Sprite[] MagicWeaponSprites;
    [SerializeField] private GameObject WeaponPrefab;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnHat(Hat hat, Vector3 location)
    {
        var HatInstance = Instantiate(HatPrefab, location, Quaternion.identity);

        var HatScript = HatInstance.GetComponent<HatGO>();

        if (HatScript != null)
        {
            HatScript.Initialize(hat);
        }
    }

    public void SpawnWeapon(Weapon weapon, Vector3 location)
    {

    }

    public Hat GetRandomHat(float levelMultiplier)
    {
        /* public Hat(ItemCategory itemCategory, Sprite sprite, float damageMultiplier, float attackSpeedMultiplier)*/

        var category = GetRandomEnumValue<ItemCategory>();

        var spriteList = category switch
        {
            ItemCategory.Melee => MeleeHatSprites,
            ItemCategory.Ranged => RangedHatSprites,
            ItemCategory.Magic => MagicHatSprites,
            _ => throw new ArgumentOutOfRangeException(nameof(ItemCategory), "Invalid weapon type.")
        };

        Sprite randomSprite;
        do
        {
            randomSprite = spriteList[UnityEngine.Random.Range(0, spriteList.Length)];
        } while (randomSprite == null); // Ensure the sprite is valid.

        float damageMultiplier = UnityEngine.Random.Range(1.0f * levelMultiplier, 3 * levelMultiplier);

        // Randomize the attack speed in a similar way, encouraging a balance between damage and speed.
        float attackSpeedMultiplier = UnityEngine.Random.Range(1.0f * levelMultiplier, 3 * levelMultiplier);

        return new Hat(category, randomSprite, damageMultiplier, attackSpeedMultiplier);
    }


    private Vector3 GetRandomSpawnPosition(GameObject map)
    {
        if (map == null)
        {
            Debug.LogError("Background object is not assigned.");
            return Vector3.zero;
        }

        var renderer = map.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("Renderer not found on the background object.");
            return Vector3.zero;
        }

        var randomX = UnityEngine.Random.Range(renderer.bounds.min.x, renderer.bounds.max.x);
        var randomY = UnityEngine.Random.Range(renderer.bounds.min.y, renderer.bounds.max.y);
        return new Vector3(randomX, randomY, 0);
    }

    public static T GetRandomEnumValue<T>() where T : Enum
    {
        // Get all possible values of the enum. This supports enums with any number of values.
        var values = Enum.GetValues(typeof(T));

        return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }



}
