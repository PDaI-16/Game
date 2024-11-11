using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public SpriteRenderer weaponRenderer; // Assign the Weapon GameObject's Sprite Renderer in Inspector
    public Sprite swordSprite;
    public Sprite axeSprite;

    public void EquipWeapon(Sprite weaponSprite)
    {
        weaponRenderer.sprite = weaponSprite;
    }

    private void Update()
    {
        // Press "1" to equip the sword
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipSword();
            Debug.Log("Sword equipped");
        }

        // Press "2" to equip the axe
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipAxe();
            Debug.Log("Axe equipped");
        }
    }

    public void EquipSword()
    {
        EquipWeapon(swordSprite);
    }

    public void EquipAxe()
    {
        EquipWeapon(axeSprite);
    }
}
