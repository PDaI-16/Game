using UnityEngine;

/// <summary>
/// Hat GameObject class including Hat : Item composite class.
/// </summary>
public class HatGO : MonoBehaviour
{
    [SerializeField] public Hat hatData;
    private bool onGround = true;

    private GameObject inventory;
    private InventoryGO inventoryGO;

    /// <summary>
    /// Initializes the HatGO with the given Hat data.
    /// </summary>
    /// <param name="hat">The Hat data to associate with this HatGO.</param>
    public void Initialize(Hat hat)
    {
        if (hat == null)
        {
            Debug.LogError("Hat data is null. Initialization failed.");
            return;
        }

        // Assign the hat data
        this.hatData = hat;

        // Attempt to find the SpriteRenderer and set its sprite
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer not found in children of {gameObject.name}. Ensure the prefab has a SpriteRenderer component.");
            return;
        }

        if (hat.Sprite == null)
        {
            Debug.LogError("Hat Sprite is null. Cannot set sprite on SpriteRenderer.");
            return;
        }

        // Set the sprite
        spriteRenderer.sprite = hat.Sprite;
        Debug.Log($"HatGO initialized successfully with category {hat.Category} and sprite {hat.Sprite.name}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && onGround == true && inventoryGO != null)
        {
            Debug.Log("Player on hat");
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
        inventoryGO.InventoryData.AddHatToInventory(hatData);

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
        // You can add HatGO-specific behavior here if needed.
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
