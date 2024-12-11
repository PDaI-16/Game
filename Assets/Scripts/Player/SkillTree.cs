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
        public ItemCategory requiredClass; // The player class required for this skill (optional)
        public bool isSpecial = false; // Indicates if the skill is a special skill
        public float damageBonus = 0f;   // Bonus to damage
        public float healthBonus = 0f;   // Bonus to health
        public float defenseBonus = 0f;  // Bonus to defense
    }

    public Skill[] skills; // Array of skills in the skill tree
    public Image[] skillImages; // Array of skill images
    public PlayerController playerController; // We are going to get playerdata from playercontroller

    // Track which special skill is currently active for each class
    public Skill meleeSpecialSkill = null;
    public Skill rangedSpecialSkill = null;
    public Skill magicSpecialSkill = null;

    void Start()
    {
        if (playerController.playerData == null)
        {
            Debug.LogError("PlayerData not found by SkillTree");
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
        ItemCategory playerClass = MainMenu.playerClass;
        Debug.LogWarning("UnlockFirstSkillBasedOnClass");
        Debug.LogWarning(MainMenu.playerClass);
        Debug.LogWarning(playerController.playerData.GetInitialPlayerClassFromPlayer().ToString());


        // Iterate through the skills to find the first skill for the player's class
        for (int i = 0; i < skills.Length; i++)
        {
            Debug.LogWarning("Skill in index");
            Debug.LogWarning(skills[i].requiredClass.ToString());
            if (skills[i].requiredClass == playerClass)
            {
                Debug.LogWarning("skills[i].requiredClass == playerClass ");

                if (!skills[i].isUnlocked)
                {
                    Debug.LogWarning("!skills[i].isUnlocked ");
                    skills[i].isUnlocked = true; // Unlock the skill
                    UpdateSkillVisuals(); // Update UI visuals
                    break; // Stop after unlocking the first skill for the class
                }

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

        if (CanUnlockSkill(clickedSkill))
        {
            UnlockSkill(clickedSkill);

            // If it's a special skill, toggle it between active and inactive
            if (clickedSkill.isSpecial)
            {
                ToggleSpecialSkill(clickedSkill);
            }
        }
        else
        {
            Debug.Log("Not enough points or prerequisites not met.");
        }
    }
    void ToggleSpecialSkill(Skill skill)
    {
        Debug.Log($"Toggling special skill: {skill.name}, Class: {skill.requiredClass}");
        if (!skill.isUnlocked)
        {
            // If the skill isn't unlocked, do nothing
            return;
        }

        // Toggle the special skill's active state
        if (skill.requiredClass == ItemCategory.Melee)
        {
            if (meleeSpecialSkill == skill)
            {
                // Deactivate the skill if it's already active
                meleeSpecialSkill = null;
            }
            else
            {
                // Activate the skill and deactivate any previously active skill
                meleeSpecialSkill = skill;
            }
        }
        else if (skill.requiredClass == ItemCategory.Ranged)
        {
            if (rangedSpecialSkill == skill)
            {
                // Deactivate the skill if it's already active
                rangedSpecialSkill = null;
            }
            else
            {
                // Activate the skill and deactivate any previously active skill
                rangedSpecialSkill = skill;
            }
        }
        else if (skill.requiredClass == ItemCategory.Magic)
        {
            if (magicSpecialSkill == skill)
            {
                // Deactivate the skill if it's already active
                magicSpecialSkill = null;
            }
            else
            {
                // Activate the skill and deactivate any previously active skill
                magicSpecialSkill = skill;
            }
        }

        UpdateSkillVisuals();  // Update the visuals to reflect the new active skill
    }

    bool CanUnlockSkill(Skill skill)
    {
        int availableSkillPoints = playerController.playerData.GetSkillPoints();

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
        if (skill.cost <= playerController.playerData.GetSkillPoints())
        {
            if (skill.isUnlocked)
            {
                // If the skill is already unlocked, no need to do anything
                return;
            }

            // Unlock the skill
            skill.isUnlocked = true;

            // Apply stat bonuses to the player
            ApplyStatBonuses(skill);

            // Deduct points only when the skill is first unlocked
            playerController.playerData.SpendSkillPoints(skill.cost);

            // If it's a special skill, set it as active
            if (skill.isSpecial)
            {
                SetActiveSpecialSkill(skill); // Set the unlocked special skill as active
            }

            UpdateSkillVisuals();  // Update UI visuals
        }
    }
    void ApplyStatBonuses(Skill skill)
    {
        MeleeAttackGO meleeAttackGO = FindFirstObjectByType<MeleeAttackGO>();
        if (meleeAttackGO == null)
        {
            Debug.LogWarning("MeleeAttackGO not found in the scene.");
            return;
        }
        // If the skill provides a damage bonus, apply it to the player's damage
        if (skill.damageBonus > 0f)
        {
            meleeAttackGO.SetSkillDamageBonus(skill.damageBonus);
            Debug.Log($"Damage bonus from skill applied: {skill.damageBonus}");
        }

        // If the skill provides a health bonus, apply it to the player's health using the setter
        if (skill.healthBonus > 0f)
        {
            // Use the SetHealth method to apply the health bonus.
            float newHealth = playerController.playerData.GetHealth() + skill.healthBonus;
            playerController.playerData.SetHealth(newHealth);
            Debug.Log($"Health increased by {skill.healthBonus}. New health: {playerController.playerData.GetHealth()}");
        }

        // If the skill provides a defense bonus, apply it to the player's defense using the setter
        if (skill.defenseBonus > 0f)
        {
            // Use the SetDefence method to apply the defense bonus.
            float newDefense = playerController.playerData.GetDefence() + skill.defenseBonus;
            playerController.playerData.SetDefence(newDefense);
            Debug.Log($"Defense increased by {skill.defenseBonus}. New defense: {playerController.playerData.GetDefence()}");
        }

        // Add more stat bonuses as needed (e.g., mana, speed, etc.)
    }

    void SetActiveSpecialSkill(Skill skill)
    {
        // Deactivate any previous special skill of the same class
        if (skill.requiredClass == ItemCategory.Melee)
        {
            if (meleeSpecialSkill == skill)
            {
                // Deactivate the skill if it's already active
                meleeSpecialSkill = null;
            }
            else
            {
                meleeSpecialSkill = skill; // Set the new melee special skill
            }
        }
        else if (skill.requiredClass == ItemCategory.Ranged)
        {
            if (rangedSpecialSkill == skill)
            {
                // Deactivate the skill if it's already active
                rangedSpecialSkill = null;
            }
            else
            {
                rangedSpecialSkill = skill; // Set the new ranged special skill
            }
        }
        else if (skill.requiredClass == ItemCategory.Magic)
        {
            if (magicSpecialSkill == skill)
            {
                // Deactivate the skill if it's already active
                magicSpecialSkill = null;
            }
            else
            {
                magicSpecialSkill = skill; // Set the new magic special skill
            }
        }

        UpdateSkillVisuals();  // Update the visuals to reflect the new active skill
    }


    void UpdateSkillVisuals()
    {
        for (int i = 0; i < skillImages.Length; i++)
        {
            if (skillImages[i] == null)
            {
                continue; // Skip this iteration if the image is not assigned
            }

            Skill skill = skills[i];
            if (skill == null)
            {
                continue;
            }

            if (skill.isUnlocked)
            {
                skillImages[i].color = Color.green; // Change the image color to green for unlocked skills

                // Highlight active special skill in blue
                if (skill.isSpecial && IsSpecialSkillActive(skill))
                {
                    skillImages[i].color = Color.blue;
                }
            }
            else
            {
                skillImages[i].color = Color.gray; // Change the image color to gray for locked skills
            }
        }
    }


    bool IsSpecialSkillActive(Skill skill)
    {
        if (skill.requiredClass == ItemCategory.Melee && meleeSpecialSkill == skill)
        {
            return true;
        }
        else if (skill.requiredClass == ItemCategory.Ranged && rangedSpecialSkill == skill)
        {
            return true;
        }
        else if (skill.requiredClass == ItemCategory.Magic && magicSpecialSkill == skill)
        {
            return true;
        }

        return false;
    }

    void Update()
    {
        // Detect mouse button 1 (left-click) to toggle active special skill
        if (Input.GetMouseButtonDown(0)) // Mouse 1 (left click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                int skillIndex = GetSkillIndexFromObject(clickedObject);

                if (skillIndex >= 0 && skillIndex < skills.Length)
                {
                    Skill clickedSkill = skills[skillIndex];

                    // If the skill is unlocked and is special, set it as active or toggle it off
                    if (clickedSkill.isUnlocked && clickedSkill.isSpecial)
                    {
                        SetActiveSpecialSkill(clickedSkill);
                        UpdateSkillVisuals(); // Update visuals after toggling
                    }
                }
            }
        }
    }

    // Helper function to get the skill index based on the clicked object
    int GetSkillIndexFromObject(GameObject clickedObject)
    {
        for (int i = 0; i < skillImages.Length; i++)
        {
            if (clickedObject == skillImages[i].gameObject)
            {
                return i;
            }
        }
        return -1; // Return -1 if the object is not found
    }
}
