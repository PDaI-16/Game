using UnityEngine;

/// <summary>
/// Hat GameObject class including Hat : Item composite class.
/// </summary>
/// <summary>
/// Hat GameObject class including Hat : Item composite class.
/// </summary>
public class HatGO : ItemGO<Hat>
{
    protected override void AddItemToInventory(Hat item)
    {
        inventoryGO.InventoryData.AddHatToInventory(item);
    }
}
