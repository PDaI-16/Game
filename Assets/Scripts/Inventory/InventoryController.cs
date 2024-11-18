using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Weapon> weaponsInInventory = new List<Weapon>();
/*    public List<GameObject> hatsInInventory = new List<GameObject>(); // todo hats*/


    public void AddWeapon(Weapon pickedUpWeapon)
    {
        /*        weaponsInInventory.Add(pickedUpWeapon);*/
        Debug.Log("In function: addWeapon");
        weaponsInInventory.Add(pickedUpWeapon);
    }

    public void RemoveWeapon(Weapon droppedWeapon)
    {
        weaponsInInventory.Remove(droppedWeapon);
    }
}
