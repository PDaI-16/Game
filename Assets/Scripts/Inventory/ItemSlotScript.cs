using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotScript : MonoBehaviour
{
    [SerializeField] private Sprite equippedSprite;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Image itemSlotImage;
    [SerializeField] private Image rarityImage;

    private GameObject itemSpawner = null;

    private bool previousCurrentWeaponEqual = false;
    private bool previousCurrentHatEqual = false;

    private PlayerController playerController = null;
    private Weapon weapon = null;
    private Hat hat = null;


    private enum ItemType {None, Weapon, Hat }
    private ItemType selfItemType = ItemType.None;

    void Start()
    {
        
    }

    void Update()
    {
        if (selfItemType != ItemType.None)
        {
            // Check if the item type matches and if it's different from the last state
            switch (selfItemType)
            {
                case ItemType.Weapon:
                    HandleItemEquipChange(playerController.GetCurrentWeaponData() == weapon, ref previousCurrentWeaponEqual);
                    break;
                case ItemType.Hat:
                    HandleItemEquipChange(playerController.GetCurrentHatData() == hat, ref previousCurrentHatEqual);
                    break;
            }
        }
    }

    // Generalized method for checking item equip state and updating the sprite
    private void HandleItemEquipChange(bool isEquipped, ref bool previousState)
    {
        // If the state of the equipped item changes, update the sprite
        if (isEquipped != previousState)
        {
            UpdateItemSprite(isEquipped);
            previousState = isEquipped;
        }
    }

    // Method to update the sprite based on equipped state
    private void UpdateItemSprite(bool isEquipped)
    {
        itemSlotImage.sprite = isEquipped ? equippedSprite : defaultSprite;
    }

    public void EquipItemToPlayer()
    {
        if (selfItemType == ItemType.Weapon && playerController != null && weapon != null)
        {
            playerController.EquipWeapon(weapon);
        }
        else if (selfItemType == ItemType.Hat && playerController != null && hat != null)
        {
            playerController.EquipHat(hat);
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

        itemSpawner = GameObject.Find("ItemSpawner");

        if (itemSpawner != null)
        {
            rarityImage.color = itemSpawner.GetComponent<ItemSpawner>().GetRarityColor(weapon.Type, weapon.ItemScore);
        }
        else
        {
            Debug.LogWarning("ItemSlotScript did not find itemspawner");
        }

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

        if (itemSpawner != null)
        {
            rarityImage.color = itemSpawner.GetComponent<ItemSpawner>().GetRarityColor(hat.Type, hat.ItemScore);
        }
        else
        {
            Debug.LogWarning("ItemSlotScript did not find itemspawner");
        }
        

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
