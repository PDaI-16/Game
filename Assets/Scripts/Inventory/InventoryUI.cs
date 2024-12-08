using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventoryGO inventoryGO;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject contentParent;
    [SerializeField] TextMeshProUGUI attackSpeedGUI;

    private int previousWeaponCount = 0;


    [SerializeField] Inventory inventoryData = null;
    private List<Weapon> weaponsInInventory = new List<Weapon>();
    private List<Hat> hatsInInventory = new List<Hat>();

    private enum ItemType { Weapons, Hats }
    private ItemType currentItemType = ItemType.Weapons;


    void Start()
    {

        FetchInventoryData();


    }

    // Update is called once per frame
    void Update()
    {
        
        if (previousWeaponCount != inventoryData.GetWeaponsInInventory().Count)
        {
            FetchInventoryData();

            MapItems(inventoryData);
        }
    }

    public void ShowWeapons()
    {
        currentItemType = ItemType.Weapons;
        MapItems(inventoryData);
    }

    public void ShowHats()
    {
        currentItemType = ItemType.Hats;
        MapItems(inventoryData);
    }



    private void FetchInventoryData()
    {
        previousWeaponCount = inventoryGO.InventoryData.GetWeaponsInInventory().Count; // Previous weapon count is saved so that we know if we have to update the mappings.
        inventoryData = inventoryGO.InventoryData;
    }

    private void MapItems(Inventory playerInventoryData)
    {
        // Clear current inventory UI items
        foreach (Transform child in contentParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Fetch the correct list of items based on the selected type
        if (currentItemType == ItemType.Weapons)
        {
            weaponsInInventory = playerInventoryData.GetWeaponsInInventory();

            if (weaponsInInventory != null)
            {
                foreach (Weapon weapon in weaponsInInventory)
                {
                    GameObject itemInInventoryUI = Instantiate(itemSlotPrefab, contentParent.transform);
                    ItemSlotScript itemSlotScript = itemInInventoryUI.GetComponent<ItemSlotScript>();
                    if (itemSlotScript != null)
                    {
                        itemSlotScript.SetWeapon(weapon);
                    }
                    else
                    {
                        Debug.LogError("ItemSlotScript is missing on the instantiated prefab!");
                    }
                }
            }
            else
            {
                Debug.LogWarning("No weapons in inventory");
            }
        }
        else if (currentItemType == ItemType.Hats)
        {
            hatsInInventory = playerInventoryData.GetHatsInInventory(); // Assuming you have a method for this

            if (hatsInInventory != null)
            {
                foreach (Hat hat in hatsInInventory)
                {
                    GameObject itemInInventoryUI = Instantiate(itemSlotPrefab, contentParent.transform);
                    ItemSlotScript itemSlotScript = itemInInventoryUI.GetComponent<ItemSlotScript>();
                    if (itemSlotScript != null)
                    {
                        itemSlotScript.SetHat(hat); // Assuming you have a SetHat method
                    }
                    else
                    {
                        Debug.LogError("ItemSlotScript is missing on the instantiated prefab!");
                    }
                }
            }
            else
            {
                Debug.LogWarning("No hats in inventory");
            }
        }
    }



}
