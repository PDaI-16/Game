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


    private Inventory inventoryData = null;
    private List<Weapon> weaponsInInventory = new List<Weapon>();

    void Start()
    {
        FetchInventoryData();
        GameObject newItem = Instantiate(itemSlotPrefab, contentParent.transform);
    }

    // Update is called once per frame
    void Update()
    {
        MapItems(inventoryData);

    }



    private void FetchInventoryData()
    {
        inventoryData = inventoryGO.InventoryData;
    }

    private void MapItems(Inventory playerInventoryData)
    {
        weaponsInInventory = playerInventoryData.GetWeaponsInInventory();

        if (weaponsInInventory != null)
        {
            foreach (Weapon weapon in weaponsInInventory)
            {
                GameObject itemInInventoryUI = Instantiate(itemSlotPrefab, contentParent.transform);
                SetAttackSpeedText(itemInInventoryUI, weapon.AttackSpeed);
            }
        }



    }


    private void SetAttackSpeedText(GameObject itemSlotObject, float attackSpeedOfItem)
    {
        // Find the child GameObject with the name "AttackSpeedText"
        Transform attackSpeedTextTransform = itemSlotObject.transform.Find("AttackSpeedText");

        // Check if the GameObject is found
        if (attackSpeedTextTransform != null)
        {
            // Get the TextMeshProUGUI component from the found GameObject
            TextMeshProUGUI attackSpeedTextMeshPro = attackSpeedTextTransform.GetComponent<TextMeshProUGUI>();

            // Check if the TextMeshProUGUI component is found
            if (attackSpeedTextMeshPro != null)
            {
                // Modify the text of the TextMeshProUGUI component
                attackSpeedTextMeshPro.text = attackSpeedOfItem.ToString(); 
            }
            else
            {
                Debug.LogWarning("No TextMeshProUGUI component found in AttackSpeedText.");
            }
        }
        else
        {
            Debug.LogWarning("No child named 'AttackSpeedText' found in itemSlotObject.");
        }
    }
}
