using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour
{
    public Sprite HearthFull;
    public Sprite HearthThreeQuarters;
    public Sprite HearthHalf;
    public Sprite HearthQuarter;
    public Sprite HearthEmpty;

    public Image heartImage; // Reference to the Image component in HealthUI

    public Image meleeSkillImage;  // UI slot for melee skill
    public Image rangedSkillImage; // UI slot for ranged skill
    public Image magicSkillImage;  // UI slot for magic skill

    [SerializeField] private PlayerStats playerStats;  // Reference to PlayerStats
    [SerializeField] private SkillTree skillTree;      // Reference to SkillTree
    public Sprite GUI1_0;  // Placeholder sprite for special skills

    void Start()
    {
        UpdateHealthImage();
        UpdateSkillIcons();
    }

    void Update()
    {
        UpdateHealthImage();
        UpdateSkillIcons();
    }

    public void UpdateHealthImage()
    {
        if (playerStats.health >= (playerStats.maxHealth * 76) / 100)
        {
            heartImage.sprite = HearthFull;
        }
        else if (playerStats.health >= (playerStats.maxHealth * 51) / 100)
        {
            heartImage.sprite = HearthThreeQuarters;
        }
        else if (playerStats.health >= (playerStats.maxHealth * 26) / 100)
        {
            heartImage.sprite = HearthHalf;
        }
        else if (playerStats.health >= (playerStats.maxHealth * 1) / 100)
        {
            heartImage.sprite = HearthQuarter;
        }
        else
        {
            heartImage.sprite = HearthEmpty;
        }
    }

    public void UpdateSkillIcons()
    {
        UpdateSkillIcon(skillTree.meleeSpecialSkill, meleeSkillImage);
        UpdateSkillIcon(skillTree.rangedSpecialSkill, rangedSkillImage);
        UpdateSkillIcon(skillTree.magicSpecialSkill, magicSkillImage);
    }


    private void UpdateSkillIcon(SkillTree.Skill skill, Image skillImage)
    {
        // Use the placeholder sprite for null or invalid skills
        Sprite skillSprite = GetSkillSprite(skill);

        // Update the image with the obtained sprite
        skillImage.sprite = skillSprite;

        // Highlight owned skills (non-placeholder) in blue; otherwise, default to white
        skillImage.color = skillSprite == GUI1_0 ? Color.white : Color.blue;
    }

    private Sprite GetSkillSprite(SkillTree.Skill skill)
    {
        // If the skill is null, return the placeholder sprite
        if (skill == null)
        {
            return GUI1_0; // Default placeholder sprite
        }

        // Locate the skill in the array
        int index = System.Array.IndexOf(skillTree.skills, skill);

        // Validate index and return the sprite if valid
        if (index >= 0 && index < skillTree.skillImages.Length)
        {
            return skillTree.skillImages[index].sprite;
        }

        // If skill is not found or index is invalid, silently return the placeholder
        return GUI1_0;
    }

}
