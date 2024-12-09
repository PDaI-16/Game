using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotScript : MonoBehaviour
{
    private PlayerController playerController = null;
    [SerializeField] private Weapon weapon = null;
    [SerializeField] private Hat hat = null;


    private enum ItemType {None, Weapon, Hat }
    private ItemType selfItemType = ItemType.None;

    void Start()
    {
    }

    void Update()
    {
    }

    public void EquipItemToPlayer()
    {
        if (selfItemType == ItemType.Weapon && playerController != null)
        {
            playerController.EquipWeapon(weapon);
        }
        else if (selfItemType == ItemType.Hat && playerController != null)
        {
            Debug.LogError("Equip hats functionality is not done yet...");
        }
    }

    public void SetWeapon(Weapon weapondata, PlayerController playerControllerinpanel)
    {
        playerController = playerControllerinpanel;

        weapon = weapondata;
        selfItemType = ItemType.Weapon;

        // Call the modular SetText function for attack speed, damage, and category
        SetText("AttackSpeedText", weapondata.AttackSpeed.ToString("F2"));
        SetText("DamageText", weapondata.Damage.ToString("F2"));
        SetText("ItemCategoryText", weapondata.Category.ToString());
        SetText("ScoreText", weapondata.ItemScore.ToString("F2"));
        SetImage(weapondata.Sprite);
        
    }

    public void SetHat(Hat hatdata, PlayerController playerControllerinpanel)
    {
        playerController = playerControllerinpanel;

        hat = hatdata;
        selfItemType = ItemType.Hat;


        // Call the modular SetText function for attack speed, damage, and category
        SetText("AttackSpeedText", "+"+hatdata.AttackSpeedMultiplier.ToString("F2"));
        SetText("DamageText", "+"+hatdata.DamageMultiplier.ToString("F2"));
        SetText("ItemCategoryText", hatdata.Category.ToString());
        SetText("ScoreText", hatdata.ItemScore.ToString("F2"));
        SetImage(hatdata.Sprite);

    }

    public Weapon GetWeapon()
    {
        return weapon;
    }

    public Hat GetHat()
    {
        return hat;
    }

    private void SetImage(Sprite itemImage)
    {
        Transform itemImageTransform = transform.Find("HorizontalLayout").transform.Find("ItemImage");

        if (itemImageTransform != null)
        {
            Image imageComponent = itemImageTransform.GetComponent<Image>();

            if (imageComponent != null)
            {
                imageComponent.sprite = itemImage;
            }
            else
            {
                Debug.LogWarning("No Image component found in 'ItemImage'.");
            }
        }
        else
        {
            Debug.LogWarning("No child named 'ItemImage' found.");
        }
    }

    // Modular method to set the text for any UI element
    private void SetText(string childName, string textValue)
    {
        Transform textTransform = transform.Find("HorizontalLayout").transform.Find(childName);

        if (textTransform != null)
        {
            TextMeshProUGUI textObject = textTransform.GetComponent<TextMeshProUGUI>();

            if (textObject != null)
            {
                textObject.text = textValue;
            }
            else
            {
                Debug.LogWarning($"No TextMeshProUGUI component found in {childName}.");
            }
        }
        else
        {
            Debug.LogWarning($"No child named '{childName}' found in itemSlotObject.");
        }
    }
}
