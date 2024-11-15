using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    // Array of sprites for possible weapon visuals
    [SerializeField] private Sprite[] possibleSprites;

    // Enum definition for weapon types
    public enum WeaponType
    {
        Melee,
        Ranged,
        Magic
    }

    // Field for selecting weapon type in the Inspector
    [SerializeField] private WeaponType weaponType;

    // Weapon stats
    [SerializeField] private float baseDamge = 3.0f;
    [SerializeField] private float baseAttackSpeed = 3.0f;


    // Optional: Public getters for the fields if needed
    public Sprite[] PossibleSprites => possibleSprites;
    public WeaponType Type => weaponType;
    public float BaseDamage => baseDamge;
    public float BaseAttackSpeed => baseAttackSpeed;


}