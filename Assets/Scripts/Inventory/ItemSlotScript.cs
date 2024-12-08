using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotScript : MonoBehaviour
{

    [SerializeField] private Weapon weapon = null;
    [SerializeField] private Hat hat = null;


    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWeapon(Weapon weapondata)
    {
        if (hat == null)
        {
            weapon = weapondata;
        }

        SetAttackSpeedText(weapondata.AttackSpeed);
        SetDamageText(weapondata.Damage);
        SetImage(weapondata.Sprite);
        SetCategoryText(weapondata.Category);
       
    }

    public Weapon GetWeapon()
    {
        return weapon;
    }

    /*    public void SetHat(Hat hatdata)
        {
            if (weapon == null)
            {
                hat = hatdata;
            }

        }

        public Hat GetHat()
        {
            return hat;
        }
    */



    private void SetImage(Sprite itemImage)
    {
        // Find the child GameObject with the name "ItemImage"
        Transform itemImageTransform = transform.Find("ItemImage");

        // Check if the GameObject is found
        if (itemImageTransform != null)
        {
            // Get the Image component from the found GameObject
            Image imageComponent = itemImageTransform.GetComponent<Image>();

            // Check if the Image component is found
            if (imageComponent != null)
            {
                // Set the sprite of the Image component
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

    private void SetCategoryText(ItemCategory category)
    {
        // Find the child GameObject with the name "AttackSpeedText"
        Transform categoryTextTransform = transform.Find("ItemCategoryText");

        // Check if the GameObject is found
        if (categoryTextTransform != null)
        {
            // Get the TextMeshProUGUI component from the found GameObject
            TextMeshProUGUI textObject = categoryTextTransform.GetComponent<TextMeshProUGUI>();

            // Check if the TextMeshProUGUI component is found
            if (textObject != null)
            {
                // Modify the text of the TextMeshProUGUI component
                textObject.text = category.ToString();
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

    private void SetDamageText(float damage)
    {
        // Find the child GameObject with the name "AttackSpeedText"
        Transform damageTextTransform = transform.Find("DamageText");

        // Check if the GameObject is found
        if (damageTextTransform != null)
        {
            // Get the TextMeshProUGUI component from the found GameObject
            TextMeshProUGUI attackSpeedTextMeshPro = damageTextTransform.GetComponent<TextMeshProUGUI>();

            // Check if the TextMeshProUGUI component is found
            if (attackSpeedTextMeshPro != null)
            {
                // Modify the text of the TextMeshProUGUI component
                attackSpeedTextMeshPro.text = damage.ToString("F2");
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

    private void SetAttackSpeedText(float attackspeed)
    {
        // Find the child GameObject with the name "AttackSpeedText"
        Transform attackSpeedTextTransform  = transform.Find("AttackSpeedText");

        // Check if the GameObject is found
        if (attackSpeedTextTransform != null)
        {
            // Get the TextMeshProUGUI component from the found GameObject
            TextMeshProUGUI attackSpeedTextMeshPro = attackSpeedTextTransform.GetComponent<TextMeshProUGUI>();

            // Check if the TextMeshProUGUI component is found
            if (attackSpeedTextMeshPro != null)
            {
                // Modify the text of the TextMeshProUGUI component
                attackSpeedTextMeshPro.text = attackspeed.ToString("F2");
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
