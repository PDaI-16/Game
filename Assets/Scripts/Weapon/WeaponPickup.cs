
using UnityEditor;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject Inventory;
    public InventoryController InventoryScript;
    public GameObject copyOfSelf;

    private void Start()
    {
        copyOfSelf = this.gameObject;

        Inventory = GameObject.FindWithTag("Inventory");

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

                InventoryScript.AddWeapon(copyOfSelf);
                Weapon.Destroy(this.gameObject);
            }
            else
            {
                Debug.LogWarning("InventoryScript is not assigned or doesn't have AddWeapon.");
            }

        }
    }
}