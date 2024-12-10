using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private float health = 10000;
    [SerializeField] private float defence = 0;

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
        if (cost < skillPoints)
        {
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
    public void SetHealth(float newHealth)
    {
        health = Mathf.Max(0, newHealth); // Ensure health doesn't go below 0
    }

    public void AddHealth(float amount)
    {
        // Increase the player's maxHealth and heal the player by the same amount
        float newMaxHealth = maxPossibleHealth + amount;
        maxPossibleHealth = newMaxHealth;
        health += amount;
        health = Mathf.Min(health, maxPossibleHealth);
        health = Mathf.Max(0, health);
    }
    public float GetDefence()
    {
        return defence;
    }

    public void SetDefence(float newDefence)
    {
        defence = Mathf.Max(0, newDefence); // Ensure defence doesn't go below 0
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
            // Calculate damage reduction based on defense
            float damageAfterDefense = damage - defence;

            // Ensure that damage is not negative (defense can't heal the player)
            damageAfterDefense = Mathf.Max(damageAfterDefense, 0f);

            // Subtract the reduced damage from the player's health
            health -= damageAfterDefense;

            // Log the result (optional for debugging)
            Debug.Log($"Player took {damageAfterDefense} damage after defense. Health remaining: {health}");
        }
    }



}
