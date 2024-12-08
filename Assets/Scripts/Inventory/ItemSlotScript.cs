using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotScript : MonoBehaviour
{
    [SerializeField] private Weapon weapon = null;
    [SerializeField] private Hat hat = null;

    void Start()
    {
    }

    void Update()
    {
    }

    public void SetWeapon(Weapon weapondata)
    {
        if (hat == null)
        {
            weapon = weapondata;
        }

        // Call the modular SetText function for attack speed, damage, and category
        SetText("AttackSpeedText", weapondata.AttackSpeed.ToString("F2"));
        SetText("DamageText", weapondata.Damage.ToString("F2"));
        SetText("ItemCategoryText", weapondata.Category.ToString());
        SetText("ScoreText", weapondata.ItemScore.ToString("F2"));
        SetImage(weapondata.Sprite);
    }

    public Weapon GetWeapon()
    {
        return weapon;
    }

    private void SetImage(Sprite itemImage)
    {
        Transform itemImageTransform = transform.Find("ItemImage");

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
        Transform textTransform = transform.Find(childName);

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
