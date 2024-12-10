using TMPro;
using UnityEngine;

/// <summary>
/// Weapon GameObject class including Weapon : Item composite class.
/// </summary>
public class WeaponGO : ItemGO<Weapon>
{
    protected override void AddItemToInventory(Weapon item)
    {
        inventoryGO.InventoryData.AddWeaponToInventory(item);
    }
}
