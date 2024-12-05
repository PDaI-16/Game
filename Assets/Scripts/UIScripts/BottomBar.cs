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

    [SerializeField] private PlayerController playerController; 

    [SerializeField] private SkillTree skillTree;     
    public Sprite GUI1_0;  

    void Start()
    {

        if (playerController == null)
        {
            Debug.LogError("PlayerController is not assigned to BottomBar!");
        }
    }

    void Update()
    {
        if (playerController.playerData != null)
        {
            //Debug.Log("Bottom bar updating...");
            UpdateHealthImage();
            UpdateSkillIcons();
            UpdateXPBar();
        }
        else
        {
            Debug.Log("Player does not exist - Bottom bar");
        }

    }

    public void UpdateHealthImage()
    {

        if (playerController.playerData.GetHealth() >= (playerController.playerData.GetMaxPossibleHealth() * 76) / 100)
        {
            heartImage.sprite = HearthFull;
        }
        else if (playerController.playerData.GetHealth() >= (playerController.playerData.GetMaxPossibleHealth() * 51) / 100)
        {
            heartImage.sprite = HearthThreeQuarters;
        }
        else if (playerController.playerData.GetHealth() >= (playerController.playerData.GetMaxPossibleHealth() * 26) / 100)
        {
            heartImage.sprite = HearthHalf;
        }
        else if (playerController.playerData.GetHealth() >= (playerController.playerData.GetMaxPossibleHealth() * 1) / 100)
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
            return GUI1_0; // Default placeholder sprite
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
        float xpPercent = playerController.playerData.GetXP() / playerController.playerData.GetXpRequiredForLevelUp();

        xpBarSlider.value = xpPercent;
        xpBarSlider.fillRect.GetComponent<Image>().color = Color.green; // Change color to green when full (for level-up)

        if (xpText != null)
        {
            xpText.text = $"{playerController.playerData.GetXP()} / {playerController.playerData.GetXpRequiredForLevelUp()} XP";
        }
    }
}
