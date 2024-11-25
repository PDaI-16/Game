using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] public List<Hat> hatsInInventory = new List<Hat>();
    [SerializeField] public List<Weapon> weaponsInInventory = new List<Weapon>();

    public void AddHatToInventory(Hat hat)
    {
        if (!hatsInInventory.Contains(hat))
        {
            hatsInInventory.Add(hat);
        }
    }

    public void AddWeaponToInventory(Weapon weapon)
    {
        if (!weaponsInInventory.Contains(weapon))
        {
            weaponsInInventory.Add(weapon);
        }
    }
}
