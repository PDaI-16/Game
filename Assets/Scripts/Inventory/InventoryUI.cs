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
        switch (currentItemType)
        {
            case ItemType.Weapons:
                Debug.LogWarning("CASE WEAPONS");

                if (previousWeaponCount != inventoryData.GetWeaponsInInventory().Count)
                {
                    ShowWeapons();
                }
                break;

            case ItemType.Hats:
                Debug.LogWarning("CASE HATS");

                if (previousHatCount != inventoryData.GetHatsInInventory().Count)
                {
                    ShowHats();
                }
                break;
        }

    }

    public void ShowWeapons()
    {
        Debug.Log("Show weapons clicked");
        currentItemType = ItemType.Weapons;
        FetchInventoryData();
        MapItems(inventoryData);
    }

    public void ShowHats()
    {
        Debug.Log("Show hats clicked");
        currentItemType = ItemType.Hats;
        FetchInventoryData();
        MapItems(inventoryData);
    }



    private void FetchInventoryData()
    {
        previousWeaponCount = inventoryGO.InventoryData.GetWeaponsInInventory().Count; // Previous weapon count is saved so that we know if we have to update the mappings.
        previousHatCount = inventoryGO.InventoryData.GetHatsInInventory().Count;
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
