using UnityEngine;

using TMPro;
using Unity.VisualScripting;

/// <summary>
/// Base class for Item GameObjects, shared functionality for both Hat and Weapon.
/// </summary>
public class ItemGO<T> : MonoBehaviour where T : Item
{
    [SerializeField] public T itemData;
    [SerializeField] private TextMeshPro infoText;

    private UIManager uiManager = null;

    public bool onGround = true;
    private bool isPlayerNear = false;

    private GameObject inventory;
    public InventoryGO inventoryGO;


    private ItemSpawner itemSpawner;

    /// <summary>
    /// Initializes the ItemGO with the given item data.
    /// </summary>
    /// <param name="item">The item data to associate with this ItemGO.</param>
    public void Initialize(T item, bool isOnGround)
    {
        onGround = isOnGround;
        infoText.text = "";

        if (isOnGround)
        {
            transform.localScale = new Vector3(1.25f, 1.25f, 0);
            infoText.text = item.ItemScore.ToString("F2");

            // Change color based on Item category
            itemSpawner = GameObject.Find("ItemSpawner").GetComponent<ItemSpawner>();

            if (itemSpawner != null)
            {
                if (item != null)
                {
                    Color rarityColor = itemSpawner.GetRarityColor(item.Type, item.ItemScore);
                    if (rarityColor != null)
                    {
                        infoText.color = rarityColor; 
                    }

                }
            }
        }

        if (item == null)
        {
            Debug.LogError("Item data is null. Initialization failed.");
            return;
        }

        this.itemData = item;

        // Set sprite
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer not found in children of {gameObject.name}. Ensure the prefab has a SpriteRenderer component.");
            return;
        }

        if (item.Sprite == null)
        {
            Debug.LogError("Item Sprite is null. Cannot set sprite on SpriteRenderer.");
            return;
        }

        spriteRenderer.sprite = item.Sprite;
        Debug.Log($"ItemGO initialized with category {item.Category} and sprite {item.Sprite.name}");
    }

    private void Start()
    {
        FindUIManagerScript();
        InitializeInventoryScript();

    }


    void Update()
    {
        // Check for E key press only if the player is near the item
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            HandleItemPickup();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && onGround && inventoryGO != null)
        {
            isPlayerNear = true;
            if (uiManager != null)
            {
                uiManager.ActivateHintPanel(); // Show the "Press E" hint
            }
            else
            {
                Debug.LogError("Ontriggerenter 2d no ui manager found");
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (uiManager != null)
            {
                uiManager.HideHintPanel(); // Show the "Press E" hint
            }
            else
            {
                Debug.LogWarning("Ontriggerexit 2d no ui manager found");
            }
        }
    }

    private void HandleItemPickup()
    {
        if (inventoryGO == null)
        {
            Debug.LogWarning("InventoryController is not assigned or invalid.");
            return;
        }

        // Add item to the inventory
        AddItemToInventory(itemData);

        onGround = false;

        // Destroy the item pickup object after adding to inventory
        Destroy(gameObject);
    }

    protected virtual void AddItemToInventory(T item)
    {
        // This should be overridden in derived classes (like HatGO, WeaponGO)
    }

    private void FindUIManagerScript()
    {
        GameObject boardManager = GameObject.Find("BoardManager");

        if (boardManager != null)
        {
            uiManager = boardManager.GetComponent<UIManager>();
        }
        else
        {
            Debug.LogError("BoardManager GameObject not found.");
        }
    }

    private void InitializeInventoryScript()
    {
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