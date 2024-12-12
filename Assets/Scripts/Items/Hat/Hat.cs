using UnityEngine;


/// <summary>
/// Stores data for Hat object.
/// </summary>

[System.Serializable]
public class Hat : Item
{
    public float DamageMultiplier;
    public float AttackSpeedMultiplier;

    // Constructor for Hat that takes damageMultiplier, attackSpeedMultiplier, and sprite
    public Hat(ItemCategory itemCategory, Sprite sprite, float damageMultiplier, float attackSpeedMultiplier, float meleebuff)
        : base(ItemType.Hat, itemCategory, sprite) // Pass actual values to the base constructor
    {

        switch (itemCategory)
        {
            case ItemCategory.Melee:
                this.DamageMultiplier = damageMultiplier * meleebuff;
                this.AttackSpeedMultiplier = attackSpeedMultiplier * meleebuff;
                this.ItemScore = ((damageMultiplier + attackSpeedMultiplier) / meleebuff); // Ensures that weapon score is normalized according to buff
                break;

            case ItemCategory.Ranged:
            case ItemCategory.Magic:
                this.DamageMultiplier = damageMultiplier;
                this.AttackSpeedMultiplier = attackSpeedMultiplier;
                this.ItemScore = damageMultiplier + attackSpeedMultiplier;
                break;
        }

    }
}