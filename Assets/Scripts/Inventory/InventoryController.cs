using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> weaponsInInventory = new List<GameObject>();
    public List<GameObject> hatsInInventory = new List<GameObject>(); // todo hats


    public void AddWeapon(GameObject pickedUpWeapon)
    {
        /*        weaponsInInventory.Add(pickedUpWeapon);*/
        Debug.Log("In function: addWeapon");
        weaponsInInventory.Add(pickedUpWeapon);
    }

    public void RemoveWeapon(GameObject droppedWeapon)
    {
        weaponsInInventory.Remove(droppedWeapon);
    }
}