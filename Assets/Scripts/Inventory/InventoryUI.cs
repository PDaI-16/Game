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
    private int previousHatCount = 0;

    private int currentWeaponCount = 0;
    private int currentHatCount = 0;

    [SerializeField] Inventory inventoryData = null;
    private List<Weapon> weaponsInInventory = new List<Weapon>();
    private List<Hat> hatsInInventory = new List<Hat>();

    private enum ItemType { Weapons, Hats }
    private ItemType currentItemType = ItemType.Weapons;


    void Start()
    {

        inventoryData = inventoryGO.InventoryData;

    }

    // Update is called once per frame
    void Update()
    {
        inventoryData = inventoryGO.InventoryData;

        // Check if the inventory has changed
        currentWeaponCount = inventoryData.GetWeaponsInInventory().Count;
        currentHatCount = inventoryData.GetHatsInInventory().Count;

        if (currentWeaponCount != previousWeaponCount || currentHatCount != previousHatCount)
        {
            MapItems(inventoryData); // Update if there's a change

            if (currentItemType == ItemType.Weapons)
            {
                previousWeaponCount = currentWeaponCount; // Store the current count
            }
            else
            {
                previousHatCount = currentHatCount;
            }
        }

    }

    public void ShowWeapons()
    {
        Debug.Log("Show weapons clicked");
        currentItemType = ItemType.Weapons;
        MapItems(inventoryData);
/*        previousWeaponCount = currentWeaponCount;*/
    }

    public void ShowHats()
    {
        Debug.Log("Show hats clicked");
        currentItemType = ItemType.Hats;
        MapItems(inventoryData);
/*        previousHatCount = previousWeaponCount;*/
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
                        Debug.LogWarning("Mapping weapons...");
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
            hatsInInventory = playerInventoryData.GetHatsInInventory(); 

            if (hatsInInventory != null)
            {
                foreach (Hat hat in hatsInInventory)
                {
                    GameObject itemInInventoryUI = Instantiate(itemSlotPrefab, contentParent.transform);
                    ItemSlotScript itemSlotScript = itemInInventoryUI.GetComponent<ItemSlotScript>();
                    if (itemSlotScript != null)
                    {
                        itemSlotScript.SetHat(hat); 
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
