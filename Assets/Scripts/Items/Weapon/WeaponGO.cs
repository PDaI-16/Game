using UnityEngine;

/// <summary>
/// Weapon GameObject class including Weapon : Item composite class.
/// </summary>
public class WeaponGO : MonoBehaviour
{
    [SerializeField] public Weapon weaponData; // Changed from 'hatData' to 'weaponData'
    private bool onGround = true;

    private GameObject inventory;
    private InventoryGO inventoryGO;

    /// <summary>
    /// Initializes the WeaponGO with the given Weapon data.
    /// </summary>
    /// <param name="weapon">The Weapon data to associate with this WeaponGO.</param>
    public void Initialize(Weapon weapon)
    {
        if (weapon == null)
        {
            Debug.LogError("Weapon data is null. Initialization failed.");
            return;
        }

        // Assign the weapon data
        this.weaponData = weapon;

        // Attempt to find the SpriteRenderer and set its sprite
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer not found in children of {gameObject.name}. Ensure the prefab has a SpriteRenderer component.");
            return;
        }

        if (weapon.Sprite == null)
        {
            Debug.LogError("Weapon Sprite is null. Cannot set sprite on SpriteRenderer.");
            return;
        }

        // Set the sprite
        spriteRenderer.sprite = weapon.Sprite;
        Debug.Log($"WeaponGO initialized successfully with category {weapon.Category} and sprite {weapon.Sprite.name}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && onGround && inventoryGO != null)
        {
            Debug.Log("Player on weapon");
            HandleItemPickup();
        }
    }

    private void HandleItemPickup()
    {
        if (inventoryGO == null)
        {
            Debug.LogWarning("InventoryController is not assigned or invalid.");
            return;
        }

        // Add weapon to the inventory
        inventoryGO.InventoryData.AddWeaponToInventory(weaponData); // Assuming AddWeaponToInventory exists in InventoryGO
        onGround = false;
        // Destroy the weapon pickup object after adding to inventory
        Destroy(gameObject);
    }

    private void Start()
    {
        InitializeInventoryScript();
    }

    // Update is called once per frame
    void Update()
    {
        // You can add WeaponGO-specific behavior here if needed.
    }

    private void InitializeInventoryScript()
    {
        // Find Inventory GameObject by tag
        inventory = GameObject.FindWithTag("Inventory");

        if (inventory != null)
        {
            inventoryGO = inventory.GetComponent<InventoryGO>();
        }
        else
        {
            Debug.LogError("Inventory GameObject not found.");
        }
    }
}
