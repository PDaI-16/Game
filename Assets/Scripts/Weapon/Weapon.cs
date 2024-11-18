using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public WeaponData WeaponData;

    public void SetValues(Sprite sprite, float damage, float attackSpeed, float weaponScore)
    {
        WeaponData = new WeaponData(sprite, damage, attackSpeed, weaponScore);

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = WeaponData.Sprite;
        }
    }

    public WeaponData GetValues()
    {
        return WeaponData;
    }
}
