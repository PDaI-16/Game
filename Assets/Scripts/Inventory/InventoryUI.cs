using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventoryGO inventoryGO;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject contentParent;
    [SerializeField] TextMeshProUGUI attackSpeedGUI;


    private Inventory inventoryData = null;

    void Start()
    {
        inventoryData = inventoryGO.InventoryData;
        GameObject newItem = Instantiate(itemSlotPrefab, contentParent.transform);
        SetDamageText(newItem);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDamageText(GameObject itemSlotObject)
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
                attackSpeedTextMeshPro.text = "50";  // Update this value as needed
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
