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
    public Weapon(ItemCategory itemCategory, Sprite sprite, float damage, float attackSpeed, float meleebuff)
        : base(ItemType.Weapon, itemCategory, sprite) // Pass actual values to the base constructor
    {
        switch (itemCategory)
        {
            case ItemCategory.Melee:
                this.Damage = damage*meleebuff;
                this.AttackSpeed = attackSpeed*meleebuff;
                this.ItemScore = damage * attackSpeed; // Ensures that weapon score is normalized according to buff
                break;

            case ItemCategory.Ranged:
            case ItemCategory.Magic:
                this.Damage = damage;
                this.AttackSpeed = attackSpeed;
                this.ItemScore = damage * attackSpeed;
                break;
        }

    }
}