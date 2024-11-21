using System;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]


// References
// https://stackoverflow.com/questions/972307/how-to-loop-through-all-enum-values-in-c // Iterating an enum

public class WeaponData
{
    public Sprite Sprite; // Unity will display this in the Inspector
    public float Damage;
    public float AttackSpeed;
    public float WeaponScore;

    public enum WeaponType
    {
        Melee,
        Ranged,
        Magic
    }
    public WeaponType weaponType;

    public WeaponData(Sprite sprite, float damage, float attackSpeed, float weaponScore)
    {

        SetWeaponType(sprite);
        this.Sprite = sprite;
        this.Damage = damage;
        this.AttackSpeed = attackSpeed;
        this.WeaponScore = weaponScore;

    }

    // Set the weapon type by reading the sprite name
    private void SetWeaponType(Sprite sprite) 
    {
        string spriteName = sprite.name;

        if (spriteName != null)
        {

            foreach (WeaponType val in Enum.GetValues(typeof(WeaponType)))
            {
                Debug.Log(val);
                if (spriteName.Contains(val.ToString()))
                {
                    Debug.Log("Sprite name contained " + val.ToString());
                    weaponType = val;

                }
            }
        }
        else
        {
            Debug.LogError("SetWeaponType could not parse name from the sprite");
        }

    }
}