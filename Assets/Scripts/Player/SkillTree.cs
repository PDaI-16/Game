using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillTree : MonoBehaviour
{
    [System.Serializable]
    public class Skill
    {
        public string name;  // Name of the skill
        public int cost;     // Cost to unlock the skill
        public int[] prerequisiteSkillIndices; // Indices of prerequisite skills (can be multiple)
        public bool isUnlocked = false; // Whether the skill is unlocked or not
        public string requiredClass; // The player class required for this skill (optional)
        public bool isSpecial = false; // Indicates if the skill is a special skill
    }

    public Skill[] skills; // Array of skills in the skill tree
    public Image[] skillImages; // Array of skill images
    public PlayerStats playerStats; // Reference to PlayerStats

    void Start()
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats is not assigned in the inspector!");
            return;
        }

        UnlockFirstSkillBasedOnClass();

        // Set up EventTriggers for each image
        for (int i = 0; i < skillImages.Length; i++)
        {
            int skillIndex = i; // Capture the index for closure
            EventTrigger eventTrigger = skillImages[i].gameObject.GetComponent<EventTrigger>();

            // Add EventTrigger if it doesn't exist
            if (eventTrigger == null)
            {
                eventTrigger = skillImages[i].gameObject.AddComponent<EventTrigger>();
            }

            // Create the PointerClick event entry
            EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };

            // Assign the callback method for clicking
            pointerClickEntry.callback.AddListener((eventData) => OnSkillClick(skillIndex));
            eventTrigger.triggers.Add(pointerClickEntry);
        }

        UpdateSkillVisuals(); // Update visuals after initializing
    }

    void UnlockFirstSkillBasedOnClass()
    {
        string playerClass = playerStats.playerClass;

        // Iterate through the skills to find the first skill for the player's class
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].requiredClass == playerClass && !skills[i].isUnlocked)
            {
                Debug.Log($"Automatically unlocking first skill for class: {playerClass}");
                skills[i].isUnlocked = true; // Unlock the skill
                UpdateSkillVisuals(); // Update UI visuals
                break; // Stop after unlocking the first skill for the class
            }
        }
    }

    void OnSkillClick(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= skills.Length)
        {
            Debug.LogError("Invalid skill index.");
            return;
        }

        Skill clickedSkill = skills[skillIndex];
        Debug.Log($"Clicked on skill: {clickedSkill.name}");

        if (CanUnlockSkill(clickedSkill))
        {
            UnlockSkill(clickedSkill);
        }
        else
        {
            Debug.Log("Not enough points or prerequisites not met.");
        }
    }

    bool CanUnlockSkill(Skill skill)
    {
        int availableSkillPoints = playerStats.GetAvailableSkillPoints();

        // Check if the player has enough points
        if (availableSkillPoints < skill.cost)
            return false;

        // Check if at least one prerequisite skill is unlocked
        if (skill.prerequisiteSkillIndices != null && skill.prerequisiteSkillIndices.Length > 0)
        {
            bool anyPrerequisiteUnlocked = false;

            foreach (int prerequisiteIndex in skill.prerequisiteSkillIndices)
            {
                if (prerequisiteIndex >= 0 && prerequisiteIndex < skills.Length && skills[prerequisiteIndex].isUnlocked)
                {
                    anyPrerequisiteUnlocked = true;
                    break;
                }
            }

            if (!anyPrerequisiteUnlocked)
                return false;
        }

        return true;
    }

    void UnlockSkill(Skill skill)
    {
        if (skill.isUnlocked)
        {
            Debug.Log($"{skill.name} is already unlocked.");
            return;
        }

        skill.isUnlocked = true;
        playerStats.SpendSkillPoints(skill.cost);
        Debug.Log($"Unlocked skill: {skill.name}");
        UpdateSkillVisuals();
    }

    void UpdateSkillVisuals()
    {
        for (int i = 0; i < skillImages.Length; i++)
        {
            Skill skill = skills[i];
            if (skill.isUnlocked)
            {
                skillImages[i].color = Color.green; // Change the image color to green for unlocked skills
            }
            else
            {
                skillImages[i].color = Color.gray; // Change the image color to gray for locked skills
            }
        }
    }
}
