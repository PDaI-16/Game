using UnityEngine;

/// <summary>
/// Stores data for Weapon object.
/// </summary>

[System.Serializable]
public class Weapon : Item
{
    public float Damage;
    public float AttackSpeed;

    // Constructor for Hat that takes damageMultiplier, attackSpeedMultiplier, and sprite
    public Weapon(ItemCategory itemCategory, Sprite sprite, float damage, float attackSpeed)
        : base(ItemType.Weapon, itemCategory, sprite) // Pass actual values to the base constructor
    {

        this.Damage = damage;
        this.AttackSpeed = attackSpeed;
        this.ItemScore = damage * attackSpeed;

    }
}