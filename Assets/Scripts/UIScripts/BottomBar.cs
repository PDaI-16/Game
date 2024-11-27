using UnityEngine;
using UnityEngine.UI; // Keep this for Slider UI components
using TMPro; // Add this for TextMeshPro

public class BottomBar : MonoBehaviour
{
    public Sprite HearthFull;
    public Sprite HearthThreeQuarters;
    public Sprite HearthHalf;
    public Sprite HearthQuarter;
    public Sprite HearthEmpty;

    public Image heartImage;

    public Image meleeSkillImage;
    public Image rangedSkillImage;
    public Image magicSkillImage;

    public Slider xpBarSlider;
    public TextMeshProUGUI xpText;

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private SkillTree skillTree;
    public Sprite GUI1_0;

    void Start()
    {
        UpdateHealthImage();
        UpdateSkillIcons();
        UpdateXPBar();
    }

    void FixedUpdate()
    {
        UpdateHealthImage();
        UpdateSkillIcons();
        UpdateXPBar();
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
        Sprite skillSprite = GetSkillSprite(skill);

        skillImage.sprite = skillSprite;
        skillImage.color = skillSprite == GUI1_0 ? Color.white : Color.blue;
    }

    private Sprite GetSkillSprite(SkillTree.Skill skill)
    {
        if (skill == null)
        {
            return GUI1_0; 
        }

        int index = System.Array.IndexOf(skillTree.skills, skill);
        if (index >= 0 && index < skillTree.skillImages.Length)
        {
            return skillTree.skillImages[index].sprite;
        }

        return GUI1_0;
    }

    public void UpdateXPBar()
    {
        // Calculate the current XP percentage
        float xpPercent = (float)playerStats.currentXP / playerStats.GetXPForNextLevel();

        xpBarSlider.value = xpPercent;
        xpBarSlider.fillRect.GetComponent<Image>().color = Color.green; // Change color to green when full (for level-up)

        if (xpText != null)
        {
            xpText.text = $"{playerStats.currentXP} / {playerStats.GetXPForNextLevel()} XP";
        }
    }
}
