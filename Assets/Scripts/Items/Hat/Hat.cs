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
    public Hat(ItemCategory itemCategory, Sprite sprite, float damageMultiplier, float attackSpeedMultiplier)
        : base(ItemType.Hat, itemCategory, sprite) // Pass actual values to the base constructor
    {

        this.DamageMultiplier = damageMultiplier;
        this.AttackSpeedMultiplier = attackSpeedMultiplier;
        this.ItemScore = damageMultiplier+attackSpeedMultiplier;
        
    }
}