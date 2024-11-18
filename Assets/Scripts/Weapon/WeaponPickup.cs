
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject Inventory;
    public InventoryController InventoryScript;
    public Weapon selfWeaponObject;

    private void Start()
    {

        Inventory = GameObject.FindWithTag("Inventory");
        selfWeaponObject = this.GetComponent<Weapon>();

        if (Inventory != null)
        {
            InventoryScript = Inventory.GetComponent<InventoryController>();
        }
        else
        {
            Debug.LogWarning("Inventory with tag 'Inventory' not found.");
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Collision happened with the weapon");
        if (other.CompareTag("Player"))
        {

            Debug.Log("Player is on weapon");

            if (InventoryScript != null)
            {

                InventoryScript.AddWeapon(selfWeaponObject.WeaponData);
                Destroy(this.gameObject);


            }
            else
            {
                Debug.LogWarning("InventoryScript is not assigned or doesn't have AddWeapon.");
            }

        }
    }
}