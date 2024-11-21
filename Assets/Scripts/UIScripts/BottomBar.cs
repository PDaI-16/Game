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

    void Start()
    {
        // Use serialized fields or explicitly assign in the Inspector
        if (playerStats == null)
        {
            playerStats = GameObject.FindObjectOfType<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats not found! Please assign it in the Inspector.");
                return;
            }
        }

        if (skillTree == null)
        {
            skillTree = GameObject.FindObjectOfType<SkillTree>();
            if (skillTree == null)
            {
                Debug.LogError("SkillTree not found! Please assign it in the Inspector.");
                return;
            }
        }

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
    SkillTree.Skill meleeSkill = null;
    SkillTree.Skill rangedSkill = null;
    SkillTree.Skill magicSkill = null;

    // Debugging: Checking unlocked skills
    Debug.Log("Checking unlocked skills...");

    foreach (var skill in skillTree.skills)
    {
        if (skill.isUnlocked && skill.name.Contains("Special")) // Special skills only
        {
            Debug.Log($"Unlocked skill: {skill.name} (Class: {skill.requiredClass})");

            if (skill.requiredClass == "Melee" && meleeSkill == null)
            {
                meleeSkill = skill;
                meleeSkillImage.sprite = GetSkillSprite(skill);
                meleeSkillImage.color = Color.white; // Make sure the image is visible
                Debug.Log($"Assigned {meleeSkill.name} to Melee slot.");
            }
            else if (skill.requiredClass == "Ranged" && rangedSkill == null)
            {
                rangedSkill = skill;
                rangedSkillImage.sprite = GetSkillSprite(skill);
                rangedSkillImage.color = Color.white;
                Debug.Log($"Assigned {rangedSkill.name} to Ranged slot.");
            }
            else if (skill.requiredClass == "Magic" && magicSkill == null)
            {
                magicSkill = skill;
                magicSkillImage.sprite = GetSkillSprite(skill);
                magicSkillImage.color = Color.white;
                Debug.Log($"Assigned {magicSkill.name} to Magic slot.");
            }
        }
    }
}

    private Sprite GetSkillSprite(SkillTree.Skill skill)
    {
        int index = System.Array.IndexOf(skillTree.skills, skill);
        return skillTree.skillImages[index].sprite;
    }

    public void ActivateMeleeSkill()
    {
        Debug.Log("Melee skill activated!");
        // Implement your activation logic here
    }

    public void ActivateRangedSkill()
    {
        Debug.Log("Ranged skill activated!");
        // Implement your activation logic here
    }

    public void ActivateMagicSkill()
    {
        Debug.Log("Magic skill activated!");
        // Implement your activation logic here
    }
}
