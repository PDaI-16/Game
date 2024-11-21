using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{


    [SerializeField] private Sprite[] possibleSprites;

    [SerializeField] private float baseDamage = 3.0f;

    [SerializeField] private float baseAttackSpeed = 3.0f;


    public Sprite[] PossibleSprites => possibleSprites;
    public float BaseDamage => baseDamage;
    public float BaseAttackSpeed => baseAttackSpeed;



    // Centralized randomization for all stats
    public (Sprite, float, float, float) GetRandomWeaponStats(int multiplier)
    {
        Sprite randomSprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
        float damage = Random.Range(1.0f, baseDamage * multiplier);
        float attackSpeed = Random.Range(1.0f, baseAttackSpeed * multiplier);
        float weaponScore = damage * attackSpeed;

        return (randomSprite, damage, attackSpeed, weaponScore);
    }
}