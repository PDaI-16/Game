using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public WeaponData WeaponData;
    [SerializeField] public bool isEquipped = false;

    public void SetValues(Sprite sprite, float damage, float attackSpeed, float weaponScore, bool boolEquip)
    {
        WeaponData = new WeaponData(sprite, damage, attackSpeed, weaponScore);
        isEquipped = boolEquip;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            print("Settings the weapons sprite");
            spriteRenderer.sprite = WeaponData.Sprite;
        }
    }

    public WeaponData GetValues()
    {
        return WeaponData;
    }
}
