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

    [SerializeField] public Inventory InventoryData;


    void Start()
    {
        InventoryData = new Inventory();
        
        defaultWeapons.Add(defaultMeleeWeapon);
        defaultWeapons.Add(defaultRangedWeapon);
        defaultWeapons.Add(defaultMagicWeapon);

        // Add all default weapons to inventory
        foreach (var weapon in defaultWeapons)
        {
            InventoryData.AddWeaponToInventory(weapon);
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
