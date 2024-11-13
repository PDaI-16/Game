using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryController", menuName = "Scriptable Objects/InventoryController")]
public class InventoryController : ScriptableObject
{
    public List<GameObject> weaponsInInventory;

    void start()
    {
        weaponsInInventory = new List<GameObject>();
    }
   
    void addWeapon(GameObject pickedUpWeapon) {
        weaponsInInventory.Add(pickedUpWeapon);
    }
 
    void removeWeapon()
    {

    }



}
