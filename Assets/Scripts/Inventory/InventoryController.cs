using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryController", menuName = "Scriptable Objects/InventoryController")]
public class InventoryController : ScriptableObject
{
    public List<GameObject> weaponsInInventory = new List<GameObject>();
    public List<GameObject> hatsInInventory = new List<GameObject>(); // todo hats
    
   
    void addWeapon(GameObject pickedUpWeapon) {
        weaponsInInventory.Add(pickedUpWeapon);
    }
 
    void removeWeapon(GameObject droppedWeapon)
    {
        weaponsInInventory.Remove(droppedWeapon);
    }

}
