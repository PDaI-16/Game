using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private float health = 100;
    [SerializeField] private int level = 1;
    [SerializeField] private float xp = 0;
    [SerializeField] private int skillPoints = 5;
    [SerializeField] private ItemCategory initialPlayerClass;

    private float xpRequiredForLevelUp = 100;
    private float requiredXPPerLevelMultiplier = 1.05f;

    private float maxPossibleHealth = 100;

    public PlayerData(ItemCategory initialClass) // InitialClass from mainmenu
    {

        this.initialPlayerClass = initialClass;
    }

    public void SetInitialClass(ItemCategory chosenClass)
    {
        initialPlayerClass = chosenClass;
    }

    public int GetSkillPoints()
    {
        return skillPoints;
    }

    public void SpendSkillPoints(int cost)
    {
        if (cost < skillPoints) {
            skillPoints -= cost;
        }
        else
        {
            Debug.Log("Not enough skillpoints");
        }
        
    }

    public ItemCategory GetInitialPlayerClassFromPlayer()
    {
        return initialPlayerClass;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetXP()
    {
        return xp;
    }

    public float GetMaxPossibleHealth()
    {
        return maxPossibleHealth;
    }

    public int GetLevel()
    {
        return level;
    }

    public float GetXpRequiredForLevelUp()
    {
        return xpRequiredForLevelUp;
    }


    public void AddXP(float xpAmount)
    {
        xp += xpAmount;
        IncreaseLevelIfPossible();
    }


    public void IncreaseLevelIfPossible()
    {
        if (xp >= xpRequiredForLevelUp)
        {
            level += 1;

            skillPoints += 1;

            // Adding the remaining xp to new level's xp
            float overflowxp = xp - xpRequiredForLevelUp;
            if (overflowxp > 0) 
            {
                xp = overflowxp;
            }
            else
            {
                xp = 0;
            }

            // Increase the required xp before level up with multiplier
            xpRequiredForLevelUp = requiredXPPerLevelMultiplier * xpRequiredForLevelUp; 
        }

    }


    public void TakeDamage(float damage)
    {
        if (health > 0f)
        {
            health -= damage;
        }
    }


    
}
