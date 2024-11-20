using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private Weapon _selfWeaponScript;

    private void Start()
    {
        InitializeInventory();
        InitializeWeaponScript();
    }

    private void InitializeInventory()
    {
        // Find Inventory GameObject by tag
        _inventory = GameObject.FindWithTag("Inventory");

        if (_inventory != null)
        {
            _inventoryController = _inventory.GetComponent<InventoryController>();
        }
        else
        {
            Debug.LogError("Inventory GameObject not found.");
        }
    }

    private void InitializeWeaponScript()
    {
        // Try to get the Weapon component attached to this GameObject
        _selfWeaponScript = GetComponent<Weapon>();

        if (_selfWeaponScript == null)
        {
            Debug.LogError("Weapon component is missing on this GameObject.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HandleWeaponPickup();
        }
    }

    private void HandleWeaponPickup()
    {
        if (_inventoryController == null)
        {
            Debug.LogWarning("InventoryController is not assigned or invalid.");
            return;
        }

        if (_selfWeaponScript == null || _selfWeaponScript.GetIsEquipped())
        {
            Debug.LogWarning("Weapon is already equipped or selfWeaponScript is missing.");
            return;
        }

        // Add weapon to the inventory
        _inventoryController.AddWeapon(_selfWeaponScript.GetWeaponData());

        // Destroy the weapon pickup object after adding to inventory
        Destroy(gameObject);
    }
}