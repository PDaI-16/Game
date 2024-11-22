using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{

    [SerializeField] private Sprite[] meleeSprites; // Sprites for melee weapons, used for visual representation.
    [SerializeField] private Sprite[] rangedSprites; // Sprites for ranged weapons.
    [SerializeField] private Sprite[] magicSprites; // Sprites for magic weapons.

    [SerializeField] private WeaponType weaponType; // Tracks the current weapon type being generated.
    [SerializeField] private float baseDamage = 3.0f; // Base damage value for weapons, to scale damage calculations.
    [SerializeField] private float baseAttackSpeed = 3.0f; // Base attack speed, affects how fast the weapon operates.

    private Sprite[] spriteList; // Temporary reference to the correct sprite list based on the weapon type.

    // Properties allow controlled access to the sprite lists and stats.
    public Sprite[] MeleeSprites => meleeSprites;
    public Sprite[] RangedSprites => rangedSprites;
    public Sprite[] MagicSprites => magicSprites;
    public float BaseDamage => baseDamage;
    public float BaseAttackSpeed => baseAttackSpeed;

    /// <summary>
    /// Generates random stats for a weapon, ensuring variety in gameplay.
    /// By centralizing the randomization logic, consistency across weapons is maintained.
    /// </summary>
    /// <param name="multiplier">Used to scale damage and attack speed based on the weapon's level or other factors.</param>
    /// <returns>
    /// A tuple containing:
    /// - WeaponType: The type of weapon chosen.
    /// - Sprite: The sprite associated with the weapon type.
    /// - float: The damage value of the weapon.
    /// - float: The attack speed of the weapon.
    /// - float: The weapon's score, which is a combined metric of its effectiveness.
    /// </returns>
    /// 

    public (WeaponType, Sprite, float, float, float) GetRandomWeaponStats(int multiplier)
    {
        // Randomly determine the weapon type to ensure diverse weapon generation.
        var weaponType = GetRandomEnumValue<WeaponType>();

        // Select the appropriate sprite list based on the weapon type.
        // This ensures that each weapon has visuals corresponding to its type.
        spriteList = weaponType switch
        {
            WeaponType.Melee => meleeSprites,
            WeaponType.Ranged => rangedSprites,
            WeaponType.Magic => magicSprites,
            _ => throw new ArgumentOutOfRangeException(nameof(weaponType), "Invalid weapon type.")
        };

        // Validate that the sprite list contains entries.
        // This prevents errors during runtime if no sprites are assigned in the Inspector.
        if (spriteList == null || spriteList.Length == 0)
        {
            Debug.LogError($"No sprites available for weapon type: {weaponType}.");
            return (weaponType, null, 0, 0, 0); // Return default values for safety.
        }

        // Choose a random sprite from the selected list to give each weapon a unique appearance.
        Sprite randomSprite;
        do
        {
            randomSprite = spriteList[UnityEngine.Random.Range(0, spriteList.Length)];
        } while (randomSprite == null); // Ensure the sprite is valid.

        // Randomize the damage value within a range, scaling it by the multiplier.
        var damage = UnityEngine.Random.Range(1.0f, baseDamage * multiplier);

        // Randomize the attack speed in a similar way, encouraging a balance between damage and speed.
        var attackSpeed = UnityEngine.Random.Range(1.0f, baseAttackSpeed * multiplier);

        // Calculate the weapon's overall score to quantify its effectiveness.
        var weaponScore = damage + attackSpeed;

        return (weaponType, randomSprite, damage, attackSpeed, weaponScore);
    }

    /// <summary>
    /// Retrieves a random value from the specified enum type.
    /// This centralizes the randomization logic for enums, making the code reusable and consistent.
    /// </summary>
    /// <typeparam name="T">The enum type to retrieve a value from.</typeparam>
    /// <returns>A randomly selected value of the specified enum type.</returns>
    /// 

    public static T GetRandomEnumValue<T>() where T : Enum
    {
        // Get all possible values of the enum. This supports enums with any number of values.
        var values = Enum.GetValues(typeof(T));

        return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }
}