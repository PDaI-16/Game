using System;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]


// References
// https://stackoverflow.com/questions/972307/how-to-loop-through-all-enum-values-in-c // Iterating an enum

public class WeaponData
{
    public WeaponType WeaponType;
    public Sprite Sprite; // Unity will display this in the Inspector
    public float Damage;
    public float AttackSpeed;
    public float WeaponScore;


    public WeaponData(WeaponType type, Sprite sprite, float damage, float attackSpeed, float weaponScore)
    {
        this.WeaponType = type;
        this.Sprite = sprite;
        this.Damage = damage;
        this.AttackSpeed = attackSpeed;
        this.WeaponScore = weaponScore;

    }

}