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
        public int prerequisiteSkillIndex = -1; // Index of the prerequisite skill (if any). Default is -1 (no prerequisite)
        public bool isUnlocked = false; // Whether the skill is unlocked or not
    }

    // List of skills in the tree
    public Skill[] skills;

    // UI elements for each skill (images or buttons)
    public Image[] skillImages;

    // Reference to PlayerStats to access available skill points
    public PlayerStats playerStats; // Make this public to assign it in the inspector

    void Start()
    {
        // Ensure PlayerStats is assigned in the Inspector
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats is not assigned in the Inspector!");
            return;
        }

        // Attach click event listeners to each skill image
        for (int i = 0; i < skillImages.Length; i++)
        {
            int skillIndex = i; // Capture the current index for the image

            // Add an EventTrigger for click events on images
            EventTrigger eventTrigger = skillImages[i].gameObject.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = skillImages[i].gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;  // Listen for click events
            entry.callback.AddListener((eventData) => OnSkillClick(skillIndex)); // Attach the callback method

            eventTrigger.triggers.Add(entry);
        }
    }

    // Method to handle skill image clicks and identify which skill was clicked
    void OnSkillClick(int skillIndex)
    {
        // Ensure the skill index is valid
        if (skillIndex < 0 || skillIndex >= skills.Length)
        {
            Debug.LogError("Invalid skill index.");
            return;
        }

        Skill clickedSkill = skills[skillIndex]; // Get the skill that was clicked

        // Print skill info to the console
        Debug.Log($"Skill clicked: {clickedSkill.name}");
        Debug.Log($"Cost: {clickedSkill.cost}");
        Debug.Log($"Is Unlocked: {clickedSkill.isUnlocked}");
        Debug.Log($"Prerequisite Skill: {(clickedSkill.prerequisiteSkillIndex >= 0 ? skills[clickedSkill.prerequisiteSkillIndex].name : "None")}");

        // Get the player's available skill points from PlayerStats
        int availableSkillPoints = playerStats.GetAvailableSkillPoints(); // Get points from PlayerStats

        // Check if the player has enough points and if prerequisites are met
        if (availableSkillPoints >= clickedSkill.cost && 
            (clickedSkill.prerequisiteSkillIndex == -1 || skills[clickedSkill.prerequisiteSkillIndex].isUnlocked))
        {
            UnlockSkill(clickedSkill);
        }
        else
        {
            Debug.Log("Not enough points or prerequisites not met.");
        }
    }

    // Unlock the skill and update the UI
    void UnlockSkill(Skill skill)
    {
        if (skill.isUnlocked)
        {
            Debug.Log(skill.name + " is already unlocked.");
            return;
        }

        skill.isUnlocked = true;
        playerStats.SpendSkillPoints(skill.cost); // Deduct points from PlayerStats
        Debug.Log($"Skill {skill.name} unlocked! Remaining points: {playerStats.GetAvailableSkillPoints()}");

        // Optionally, you can update the button visuals (e.g., disable the button, change its color, etc.)
        UpdateButtonState();
    }

    // Update the state of buttons after a skill is unlocked (e.g., disable buttons for unlocked skills)
    void UpdateButtonState()
    {
        for (int i = 0; i < skillImages.Length; i++)
        {
            Skill skill = skills[i];
            skillImages[i].GetComponent<Button>().interactable = !skill.isUnlocked; // Disable button if skill is unlocked
        }
    }
}
