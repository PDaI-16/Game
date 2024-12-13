using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private List<Hat> hatsInInventory = new List<Hat>();
    [SerializeField] private List<Weapon> weaponsInInventory = new List<Weapon>();

    public int GetItemTypeCountFromInventory(ItemType listType)
    {
        switch (listType)
        {
            case ItemType.Weapon:
                return weaponsInInventory.Count;
            case ItemType.Hat:
                return hatsInInventory.Count;
            default:
                return 0;
        }
    }

    public List<Hat> GetHatsInInventory() {
        return hatsInInventory;
    }

    public List<Hat> GetHatsOrderedByScore()
    {
        return hatsInInventory.OrderByDescending(w => w.ItemScore).ToList();
    }

    public List<Hat> GetHatsLatestFirst()
    {
        return hatsInInventory.AsEnumerable().Reverse().ToList();
    }

    public List<Weapon> GetWeaponsInInventory()
    {
        return weaponsInInventory;
    }

    public List<Weapon> GetWeaponsOrderedByScore()
    {
        return weaponsInInventory.OrderByDescending(w => w.ItemScore).ToList();
    }

    public List<Weapon> GetWeaponsLatestFirst()
    {
        return weaponsInInventory.AsEnumerable().Reverse().ToList();
    }



    public Weapon GetWeaponFromInventory(int index)
    {
        // Check if the inventory is empty
        if (weaponsInInventory == null || weaponsInInventory.Count == 0)
        {
            Debug.LogError("The weapons inventory is empty!");
            return null; // Return null if there are no weapons
        }

        // Check if the index is within bounds
        if (index < 0 || index >= weaponsInInventory.Count)
        {
            Debug.LogError($"Invalid index {index}. The index must be between 0 and {weaponsInInventory.Count - 1}.");
            return null; // Return null for an invalid index
        }

        // Return the weapon at the specified index
        return weaponsInInventory[index];
    }

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
