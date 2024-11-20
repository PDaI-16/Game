using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public Sprite Sprite; // Unity will display this in the Inspector
    public float Damage;
    public float AttackSpeed;
    public float WeaponScore;

    public WeaponData(Sprite sprite, float damage, float attackSpeed, float weaponScore)
    {
        this.Sprite = sprite;
        this.Damage = damage;
        this.AttackSpeed = attackSpeed;
        this.WeaponScore = weaponScore;
    }
}