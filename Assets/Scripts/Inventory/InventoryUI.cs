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


    [SerializeField] Inventory inventoryData = null;
    private List<Weapon> weaponsInInventory = new List<Weapon>();
    private bool wasInventoryPanelActive = false;

    void Start()
    {
        GameObject newItem = Instantiate(itemSlotPrefab, contentParent.transform);

        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the inventory panel has transitioned from inactive to active
        if (inventoryPanel.activeSelf && !wasInventoryPanelActive)
        {
            Debug.LogWarning("Mapping inventory data inside update");
            MapItems(FetchInventoryData());
        }

        // Update the panel state for the next frame
        wasInventoryPanelActive = inventoryPanel.activeSelf;
    }



    private Inventory FetchInventoryData()
    {
        return inventoryData = inventoryGO.InventoryData;
    }

    private void MapItems(Inventory playerInventoryData)
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
            Debug.LogWarning("No weapon in inventory");
        }



    }



}
