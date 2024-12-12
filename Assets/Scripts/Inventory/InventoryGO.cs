using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGO : MonoBehaviour
{
    //Default melee weapon
    private List<Weapon> defaultWeapons = new List<Weapon>();

    [SerializeField] private Weapon defaultMeleeWeapon;
    [SerializeField] private Weapon defaultRangedWeapon;
    [SerializeField] private Weapon defaultMagicWeapon;

    [SerializeField] private Hat defaultMeleeHat;
    [SerializeField] private Hat defaultRangedHat;
    [SerializeField] private Hat defaultMagicHat;

    [SerializeField] public Inventory InventoryData;

    [SerializeField] private PlayerController playerController;



    void Start()
    {
        InventoryData = new Inventory();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        
        defaultWeapons.Add(defaultMeleeWeapon);
        defaultWeapons.Add(defaultRangedWeapon);
        defaultWeapons.Add(defaultMagicWeapon);

        // Add all default weapons to inventory
        foreach (var weapon in defaultWeapons)
        {
            InventoryData.AddWeaponToInventory(weapon);
        }

        switch (MainMenu.playerClass)
        {
            case ItemCategory.Melee:
                InventoryData.AddHatToInventory(defaultMeleeHat);
                playerController.EquipHat(defaultMeleeHat);
                playerController.EquipWeapon(defaultMeleeWeapon);

                break;

            case ItemCategory.Ranged:
                InventoryData.AddHatToInventory(defaultRangedHat);
                playerController.EquipHat(defaultRangedHat);
                playerController.EquipWeapon(defaultRangedWeapon);
                break;

            case ItemCategory.Magic:
                InventoryData.AddHatToInventory(defaultMagicHat);
                playerController.EquipHat(defaultMagicHat);
                playerController.EquipWeapon(defaultMagicWeapon);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
