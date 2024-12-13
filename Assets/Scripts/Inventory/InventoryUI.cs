using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventoryGO inventoryGO;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject contentParent;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TMP_Dropdown sortingDropdown; // Reference to the Dropdown


    private int previousWeaponCount = 0;
    private int previousHatCount = 0;

    private int currentWeaponCount = 0;
    private int currentHatCount = 0;

    [SerializeField] public Inventory inventoryData;
    private List<Weapon> weaponsInInventory = new List<Weapon>();
    private List<Hat> hatsInInventory = new List<Hat>();

    private enum ItemType { Weapons, Hats }
    private ItemType currentItemType = ItemType.Weapons;

    private enum InventorySort { Latest, Oldest, Score};
    private InventorySort sortStyle = InventorySort.Score;


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

            previousWeaponCount = currentWeaponCount; // Store the current count
            previousHatCount = currentHatCount;

        }

    }

    public void OnDropdownValueChanged(int selectedIndex)
    {
        string selectedOption = sortingDropdown.options[selectedIndex].text;
        Debug.Log($"Selected option: {selectedOption}");

        switch (selectedOption)
        {
            case "LATEST":
                sortStyle = InventorySort.Latest;
                break;
            case "OLDEST":
                sortStyle = InventorySort.Oldest;
                break;
            case "SCORE":
                sortStyle = InventorySort.Score;
                break;
        }

        MapItems(inventoryData);

    }


    public void CloseInventoryPanel()
    {
        inventoryPanel.SetActive(false);
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
        Debug.LogWarning("MapItems function executed...");

        // Clear current inventory UI items
        foreach (Transform child in contentParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Fetch the correct list of items based on the selected type
        if (currentItemType == ItemType.Weapons)
        {
            switch (sortStyle)
            {
                case InventorySort.Latest:
                    weaponsInInventory = playerInventoryData.GetWeaponsLatestFirst();
                    break;
                case InventorySort.Oldest:
                    weaponsInInventory = playerInventoryData.GetWeaponsInInventory();
                    break;
                case InventorySort.Score:
                    weaponsInInventory = playerInventoryData.GetWeaponsOrderedByScore();
                    break;
            }

            if (weaponsInInventory != null)
            {
                foreach (Weapon weapon in weaponsInInventory)
                {
                    
                    GameObject itemInInventoryUI = Instantiate(itemSlotPrefab, contentParent.transform);


                    ItemSlotScript itemSlotScript = itemInInventoryUI.GetComponent<ItemSlotScript>();
                    if (itemSlotScript != null)
                    {
                        itemSlotScript.SetWeapon(weapon, playerController);
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
            switch (sortStyle)
            {
                case InventorySort.Latest:
                    hatsInInventory = playerInventoryData.GetHatsLatestFirst();
                    break;
                case InventorySort.Oldest:
                    hatsInInventory = playerInventoryData.GetHatsInInventory();
                    break;
                case InventorySort.Score:
                    hatsInInventory = playerInventoryData.GetHatsOrderedByScore();
                    break;
            }

            if (hatsInInventory != null)
            {
                foreach (Hat hat in hatsInInventory)
                {
                    GameObject itemInInventoryUI = Instantiate(itemSlotPrefab, contentParent.transform);
                    ItemSlotScript itemSlotScript = itemInInventoryUI.GetComponent<ItemSlotScript>();
                    if (itemSlotScript != null)
                    {
                        itemSlotScript.SetHat(hat, playerController); 
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
