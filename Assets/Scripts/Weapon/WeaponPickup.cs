
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] public GameObject Inventory;
    [SerializeField] public InventoryController InventoryController;
    [SerializeField] public Weapon selfWeaponScript;

    private void Start()
    {
        // Find Inventory GameObject by tag
        Inventory = GameObject.FindWithTag("Inventory");
        InventoryController = Inventory.GetComponent<InventoryController>();

        // Try to get the Weapon component attached to this GameObject
        selfWeaponScript = GetComponent<Weapon>();


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision happened with the weapon");

        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {

            // Check if InventoryScript is properly assigned
            if (InventoryController != null)
            {
                // Make sure the weapon is not already equipped before adding it to the inventory
                if (selfWeaponScript != null)
                {
                    // Add weapon to the inventory
                    InventoryController.AddWeapon(selfWeaponScript.WeaponData);
                    // Destroy the weapon pickup object after adding to inventory
                    Destroy(this.gameObject);
                }
                else
                {
                    Debug.LogWarning("Weapon is already equipped or selfWeaponObject is null.");
                }
            }
            else
            {
                Debug.LogWarning("InventoryScript is not assigned or doesn't have AddWeapon.");
            }
        }
    }
}