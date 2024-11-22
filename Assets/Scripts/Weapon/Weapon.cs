using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private bool _isEquipped = false;

    private void Start()
    {
        UpdateWeaponSprite();
    }

    public void SetValues(WeaponType weaponType, Sprite sprite, float damage, float attackSpeed, float weaponScore, bool boolEquip)
    {
        _weaponData = new WeaponData(weaponType,sprite, damage, attackSpeed, weaponScore);
        _isEquipped = boolEquip;

        UpdateWeaponSprite();
    }

    // A helper method to set the sprite for the weapon
    private void UpdateWeaponSprite()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            print("Setting the weapon's sprite");
            spriteRenderer.sprite = _weaponData.Sprite;
        }
        else
        {
            Debug.LogError("Weapon: Sprite Renderer is null");
        }
    }

    // Public getter methods
    public WeaponData GetWeaponData()
    {
        return _weaponData;
    }

    public bool GetIsEquipped()
    {
        return _isEquipped;
    }
}