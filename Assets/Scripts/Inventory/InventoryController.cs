using System.Collections.Generic;
using UnityEngine;

// https://discussions.unity.com/t/how-to-add-to-list-if-not-in-list/85674/3

public class InventoryController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   /* public List<WeaponData> weaponsInInventory = new List<WeaponData>();
    *//*    public List<GameObject> hatsInInventory = new List<GameObject>(); // todo hats*//*


    public void AddWeapon(WeaponData weaponData)
    {
        if (!weaponsInInventory.Contains(weaponData))
        {
            weaponsInInventory.Add(weaponData);
        }
        else
        {
            print("Item already in inventory");
        }
        
        foreach (var weapon in weaponsInInventory)
        {
            Debug.Log($"Damage: {weapon.Damage}, Attack Speed: {weapon.AttackSpeed}, Score: {weapon.WeaponScore}");
        }
    }

    public void RemoveWeapon(WeaponData droppedWeapon)
    {
        weaponsInInventory.Remove(droppedWeapon);
    }*/
}
